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
    public class BouncyCircle : GameObject<SimpleGameEngine>
    {
        public float Angle { get; set; }
        public float Speed { get; set; } = 300;
        public Vector2 Position { get; set; } = new Vector2(0, 0);
        public BouncyCircle()
        {
        }

        public override void Draw(CanvasDrawingSession session, SimpleGameEngine engine)
        {
            session.DrawCircle(Position, 20, Colors.Black);
        }
        bool hit = true;
        public override void Update(float totalMilliseconds, SimpleGameEngine engine)
        {
            var x = Math.Sin(Angle) * Speed * totalMilliseconds;
            var y = Math.Cos(Angle) * Speed * totalMilliseconds;
            var velocityVector = new Vector2((float)x, (float)y);

            if (Vector2.Distance(Vector2.Zero, Position) > 300 && hit)
            {
                velocityVector = Vector2.Reflect(velocityVector, Vector2.One);
            }
            Position = Position + velocityVector;
        }
    }
}