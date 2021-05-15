using Godot;
using Forces;
using Oscillation;

namespace Examples.Chapter3
{
    /// <summary>
    /// Example 3.11 - Spring Connection.
    /// </summary>
    /// Uses SimpleSpring and SimpleMover with custom UpdateAcceleration.
    public class C3Example11 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 3.11:\n"
              + "Spring Connection\n\n"
              + "You can move the ball by touching it.";
        }

        private class Mover : SimpleMover
        {
            public Mover() : base(WrapModeEnum.None)
            {
                Mass = 24;
            }

            protected override void UpdateAcceleration()
            {
                // Gravity
                ApplyForce(new Vector2(0, 0.9f));
                ApplyDamping(0.98f);
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var spring = new SimpleSpring()
            {
                Length = size.y / 2,
                MaxLength = (size.y / 2) + (size.y / 4),
                MinLength = size.y / 4,
                Position = new Vector2(size.x / 2, 0)
            };

            spring.SetMover(new Mover(), new Vector2(0, size.y / 2));

            AddChild(spring);
        }
    }
}
