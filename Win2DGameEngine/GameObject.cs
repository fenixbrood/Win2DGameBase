using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;

namespace Win2DGameEngine
{
    public abstract class GameObject<T> : IDrawable<T>, IUpdatable<T>
    {
        public int DrawLayer { get; protected set; }

        public abstract void Draw(CanvasDrawingSession session, T engine);
        public abstract void Update(float totalMilliseconds, T engine);

    }
}
