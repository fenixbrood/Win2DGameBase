using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Win2DGameEngine;
using App12.Game;

namespace App12.Game
{
    public class SimpleGameEngine : IGameEngine
    {
        public List<object> GameObjects { get; private set; } = new List<object>();
        public InputManager InputManager { get; private set; } = new InputManager();
        public Vector2 WorldSize { get; private set; } = new Vector2(500, 500);

        public SimpleGameEngine()
        {
            GameObjects.Add(new WorldBackground());
            GameObjects.Add(new StaticCircle());
            GameObjects.Add(new PlayerCircle());
        }

        public void Draw(CanvasDrawingSession drawingSession, CanvasTimingInformation timing)
        {
            GameObjects.OfType<IDrawable<SimpleGameEngine>>().OrderBy(x => x.DrawLayer).ToList().ForEach(x => x.Draw(drawingSession, this));
        }

        public async Task LoadResources(Win2dRenderer renderer)
        {
            renderer.init = true;
        }

        public void Update(TimeSpan timing)
        {
            GameObjects.OfType<IUpdatable<SimpleGameEngine>>().ToList().ForEach(x => x.Update((float)timing.TotalSeconds, this));
        }
    }
}
