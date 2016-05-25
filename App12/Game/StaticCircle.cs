using System;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Diagnostics;
using Win2DGameEngine;

namespace App12.Game
{
    public class StaticCircle : GameObject<SimpleGameEngine>
    {
        public Vector2 Position { get; set; } = new Vector2(0, 0);
        public StaticCircle()
        {
        }

        public override void Draw(CanvasDrawingSession session, SimpleGameEngine engine)
        {
            session.DrawCircle(Position, 20, Colors.Black);
        }

        public override void Update(float totalMilliseconds, SimpleGameEngine engine)
        {
 
        }
    }
}