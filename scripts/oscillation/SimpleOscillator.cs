using Godot;
using Drawing;

/// <summary>
/// Oscillation-related primitives
/// </summary>
namespace Oscillation
{
    /// <summary>
    /// Simple oscillator.
    /// </summary>
    public class SimpleOscillator : Node2D
    {
        /// <summary>Show line</summary>
        public bool ShowLine
        {
            get => lineSprite.Visible;
            set => lineSprite.Visible = value;
        }

        /// <summary>Oscillator radius</summary>
        public float Radius
        {
            get => circleSprite.Radius;
            set => circleSprite.Radius = value;
        }

        /// <summary>Oscillator angle</summary>
        public Vector2 Angle;

        /// <summary>Position offset</summary>
        public Vector2 PositionOffset;

        /// <summary>Oscillator velocity</summary>
        public Vector2 Velocity;

        /// <summary>Oscillator amplitude</summary>
        public Vector2 Amplitude;

        /// <summary>Oscillator angular acceleration</summary>
        public Vector2 AngularAcceleration;

        /// <summary>Line color</summary>
        public Color LineColor
        {
            get => lineSprite.Modulate;
            set
            {
                lineSprite.Modulate = value;
            }
        }

        /// <summary>Ball color</summary>
        public Color BallColor
        {
            get => circleSprite.Modulate;
            set
            {
                circleSprite.Modulate = value;
            }
        }

        private readonly SimpleCircleSprite circleSprite;
        private readonly SimpleLineSprite lineSprite;

        /// <summary>
        /// Create a default oscillator.
        /// </summary>
        public SimpleOscillator()
        {
            Velocity = new Vector2((float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f));
            circleSprite = new SimpleCircleSprite
            {
                Radius = 30,
                Modulate = Colors.LightCyan
            };
            lineSprite = new SimpleLineSprite
            {
                Visible = true,
                Modulate = Colors.LightGray
            };
        }

        public override void _Ready()
        {
            AddChild(lineSprite);
            AddChild(circleSprite);
        }

        public override void _Process(float delta)
        {
            Velocity += AngularAcceleration;
            Angle += Velocity;
            AngularAcceleration = Vector2.Zero;

            float x = PositionOffset.x + (Mathf.Sin(Angle.x) * Amplitude.x);
            float y = PositionOffset.y + (Mathf.Sin(Angle.y) * Amplitude.y);
            var target = new Vector2(x, y);

            lineSprite.PositionA = GlobalPosition;
            lineSprite.PositionB = GlobalPosition + target;
            circleSprite.Position = target;
        }
    }
}
