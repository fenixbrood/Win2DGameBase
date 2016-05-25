using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace Win2DGameEngine
{
    public interface IGameEngine
    {
        InputManager InputManager { get; }
        Vector2 WorldSize { get; }
        void Draw(CanvasDrawingSession drawingSession, CanvasTimingInformation timing);
        void Update(TimeSpan timing);
        Task LoadResources(Win2dRenderer renderer);
    }
}