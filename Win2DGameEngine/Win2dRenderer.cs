using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.System;
using Windows.UI.Xaml.Media;

namespace Win2DGameEngine
{
    public class Win2dRenderer : UserControl, IDisposable
    {
        public bool AllowPan { get; set; } = false;
        public bool AllowZoom { get; set; } = false;
        public bool AllowSkew { get; set; } = false;
        public IGameEngine Engine { get; private set; }
        public CanvasAnimatedControl Canvas { get; private set; } = new CanvasAnimatedControl();
        private CanvasDrawingSession drawingSession = null;
        public Vector2 ZoomFactor { get; private set; } = new Vector2(1, 1);
        public bool init = false;
        private Vector2 ActualCenter;
        public Win2dRenderer(IGameEngine engine)
        {
            this.Engine = engine;
            this.Content = Canvas;
            this.SizeChanged += Win2dRenderer_SizeChanged;
            Canvas.CreateResources += Canvas_CreateResources;
            Canvas.Update += Canvas_Update;
            Canvas.Draw += Canvas_Draw;
            Canvas.PointerMoved += canvas_PointerMoved;
            Canvas.Tapped += canvas_Tapped;
            Canvas.RightTapped += canvas_RightTapped;
            Canvas.TargetElapsedTime = new TimeSpan(166666);
            // Register for keyboard events
            Window.Current.CoreWindow.KeyDown += this.KeyDown_UIThread;
            Window.Current.CoreWindow.KeyUp += this.KeyUp_UIThread;
            this.IsTapEnabled = true;
            this.IsRightTapEnabled = true;
            Canvas.Invalidate();
        }

        private void KeyUp_UIThread(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = Engine.InputManager.Capture(args.VirtualKey);
            if (args.Handled)
            {
                var action = this.Canvas.RunOnGameLoopThreadAsync(
                () => Engine.InputManager.KeyUp(args.VirtualKey));
            }
        }

        private void KeyDown_UIThread(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = Engine.InputManager.Capture(args.VirtualKey);
            if (args.Handled)
            {
                var action = this.Canvas.RunOnGameLoopThreadAsync(
               () => Engine.InputManager.KeyDown(args.VirtualKey));
            }

            if (AllowZoom)
            {
                if (args.VirtualKey == VirtualKey.Add)
                {
                    ZoomFactor = new Vector2(ZoomFactor.X + 0.1f, ZoomFactor.Y + 0.1f);
                }
                if (args.VirtualKey == VirtualKey.Subtract)
                {
                    ZoomFactor = new Vector2(ZoomFactor.X - 0.1f, ZoomFactor.Y - 0.1f);
                }
            }
        }

        private async void Win2dRenderer_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {

            Size newSize = e.NewSize;
            var v = new Vector2((float)newSize.Width / (float)Engine.WorldSize.X, (float)newSize.Height / (float)Engine.WorldSize.Y);

            if (init)
            {
                await this.Canvas.RunOnGameLoopThreadAsync(
                () =>
                {
                    ActualCenter = new Vector2((float)newSize.Width / 2, (float)newSize.Height / 2);
                    var max = (float)Math.Min(newSize.Width / Engine.WorldSize.X, newSize.Height / Engine.WorldSize.Y);
                    ZoomFactor = new Vector2(max, max);
                });
            }
            else
            {
                ActualCenter = new Vector2((float)newSize.Width / 2, (float)newSize.Height / 2);
                var max = (float)Math.Min(newSize.Width / Engine.WorldSize.X, newSize.Height / Engine.WorldSize.Y);
                ZoomFactor = new Vector2(max, max);
            }
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            this.SetDrawingSession(args.DrawingSession);
            Engine.Draw(args.DrawingSession, args.Timing);
            this.ClearDrawingSession();
        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            Engine.Update(args.Timing.ElapsedTime);
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            init = false;
            args.TrackAsyncAction(Engine.LoadResources(this).AsAsyncAction());
        }
        private async void canvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var pos = CanvasUnitsToWorldUnits(e.GetCurrentPoint(this).Position.ToVector2());
            await this.Canvas.RunOnGameLoopThreadAsync(
                     () => this.Engine.InputManager.MousePoint = pos

                    );
        }

        private async void canvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = Engine.InputManager.Capture(VirtualKey.LeftButton);
            if (e.Handled)
            {
                await this.Canvas.RunOnGameLoopThreadAsync(
                         () => this.Engine.InputManager.KeyDown(VirtualKey.LeftButton)

                        );
            }
        }

        private async void canvas_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            e.Handled = Engine.InputManager.Capture(VirtualKey.RightButton);
            if (e.Handled)
            {
                await this.Canvas.RunOnGameLoopThreadAsync(
                     () => this.Engine.InputManager.KeyDown(VirtualKey.RightButton)
                    );
            }
        }
        public Vector2 CanvasUnitsToWorldUnits(Vector2 vector2)
        {
            var center = ActualCenter * -1;
            var res = new Vector2(center.X + vector2.X, center.Y + vector2.Y);
            return res / ZoomFactor;
        }
        private void SetDrawingSession(CanvasDrawingSession drawingSession)
        {
            this.drawingSession = drawingSession;
            if (drawingSession != null)
            {
                drawingSession.Transform = Matrix3x2.Multiply(drawingSession.Transform, Matrix3x2.CreateScale(ZoomFactor));
                drawingSession.Transform = Matrix3x2.Multiply(drawingSession.Transform, Matrix3x2.CreateTranslation(ActualCenter));           
            }
        }
        private void ClearDrawingSession()
        {
            this.SetDrawingSession(null);
        }
        public void Dispose()
        {
            ClearDrawingSession();
            Canvas.CreateResources -= Canvas_CreateResources;
            Canvas.Draw -= Canvas_Draw;
            Canvas.Update -= Canvas_Update;
            Canvas.RemoveFromVisualTree();
            Canvas = null;
            SizeChanged -= Win2dRenderer_SizeChanged;
        }
    }
}
