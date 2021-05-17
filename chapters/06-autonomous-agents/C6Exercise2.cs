using Godot;
using Agents;
using Forces;

namespace Examples.Chapter6
{
    /// <summary>
    /// Exercise 6.2: Pursuit.
    /// </summary>
    /// Same principle as Example 6.1, but using the target velocity to predict its position.
    public class C6Exercise2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 6.2:\nPursuit";
        }

        private SimpleMover targetMover;

        private class Vehicle : SimpleVehicle
        {
            public Vehicle()
            {
                MaxVelocity = 8;
                MaxForce = 0.8f;
            }

            protected override void UpdateAcceleration()
            {
                ApplyForce(Seek(Target.GlobalPosition + Target.Velocity));
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            // Create target
            targetMover = new SimpleMover()
            {
                Position = size / 2,
                Modulate = Colors.LightBlue.WithAlpha(128)
            };
            AddChild(targetMover);

            // Create vehicle
            var vehicle = new Vehicle()
            {
                Target = targetMover,
                Position = size / 4
            };
            AddChild(vehicle);
        }

        public override void _Process(float delta)
        {
            targetMover.GlobalPosition = GetViewport().GetMousePosition();
        }
    }
}
