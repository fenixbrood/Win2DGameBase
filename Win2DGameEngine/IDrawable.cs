using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win2DGameEngine;

namespace Win2DGameEngine
{
    public interface IDrawable<T>
    {
        int DrawLayer { get; }
        void Draw(CanvasDrawingSession session, T engine);
    }

    public interface IUpdatable<T>
    {
        void Update(float totalMilliseconds, T engine);
    }
}
