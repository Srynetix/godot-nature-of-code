using Godot;

namespace Examples.Chapter3
{
    /// <summary>
    /// Example 3.5 - Simple Harmonic Motion.
    /// </summary>
    /// Uses amplitude and period values (and _Draw method) to draw a moving ball.
    /// Uses frame count to limit speed.
    public class C3Example5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 3.5:\n"
              + "Simple Harmonic Motion";
        }

        private class Ball : Node2D
        {
            public float Radius = 50;

            // Amplitude == Rope length
            public float Amplitude = 250;
            // Period == Movement speed
            public float Period = 120;

            private float frameCount;

            public override void _Draw()
            {
                var x = Amplitude * Mathf.Cos(Mathf.Pi * 2 * frameCount / Period);
                var target = new Vector2(x, 0);

                DrawLine(Vector2.Zero, target, Colors.LightGray, 2);
                DrawCircle(target, Radius, Colors.LightBlue);
                DrawCircle(target, Radius - 2, Colors.White);
            }

            public override void _Process(float delta)
            {
                frameCount++;

                Update();
            }
        }

        public override void _Ready()
        {
            var ball = new Ball
            {
                Position = GetViewportRect().Size / 2
            };
            AddChild(ball);
        }
    }
}
