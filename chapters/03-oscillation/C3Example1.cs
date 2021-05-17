using Godot;

namespace Examples
{
    /// <summary>
    /// Chapter 3 - Oscillation.
    /// </summary>
    namespace Chapter3
    {
        /// <summary>
        /// Example 3.1 - Angular Motion.
        /// </summary>
        /// Uses _Draw method and manual angular velocity and acceleration management.
        public class C3Example1 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 3.1:\n"
                  + "Angular Motion";
            }

            private float angularVelocity;
            private readonly float angularAcceleration = 0.01f;
            private readonly float lineSize = 50;
            private readonly float ballRadius = 10;

            public override void _Draw()
            {
                DrawLine(Vector2.Left * lineSize, Vector2.Right * lineSize, Colors.White, 1, true);
                DrawCircle(Vector2.Left * lineSize, ballRadius, Colors.LightBlue);
                DrawCircle(Vector2.Right * lineSize, ballRadius, Colors.LightBlue);
            }

            public override void _Ready()
            {
                Position = GetViewportRect().Size / 2;
            }

            public override void _Process(float delta)
            {
                angularVelocity += angularAcceleration * delta;
                Rotation += angularVelocity;

                Update();
            }
        }
    }
}
