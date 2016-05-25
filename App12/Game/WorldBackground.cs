using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Win2DGameEngine;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;

namespace App12.Game
{
    public class WorldBackground : IDrawable<SimpleGameEngine>
    {
        public int DrawLayer { get; protected set; } = -1;

        public void Draw(CanvasDrawingSession session, SimpleGameEngine engine)
        {
            session.FillRectangle(engine.WorldSize.X / -2, engine.WorldSize.Y / -2, engine.WorldSize.X, engine.WorldSize.Y, Colors.LawnGreen);
        }
    }
}
