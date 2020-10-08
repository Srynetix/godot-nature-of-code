using Godot;
using Agents;
using Forces;

namespace Examples.Chapter6
{
    /// <summary>
    /// Exercise 6.1 - Fleeing a target.
    /// </summary>
    /// Same principle than Example 6.1, but with a flee behavior.
    public class C6Exercise1 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 6.1:\n Fleeing a target";
        }

        private SimpleMover targetMover;

        private class Vehicle : SimpleVehicle
        {
            public float FleeDistance = 200;

            public Vector2 Flee(Vector2 position)
            {
                if (Target != null)
                {
                    var targetForce = (position - GlobalPosition).Normalized() * MaxVelocity;
                    return (-targetForce - Velocity).Clamped(MaxForce);
                }
                else
                {
                    return Vector2.Zero;
                }
            }

            protected override void UpdateAcceleration()
            {
                if (GlobalPosition.DistanceSquaredTo(Target.GlobalPosition) < FleeDistance * FleeDistance)
                {
                    ApplyForce(Flee(Target.GlobalPosition));
                }
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            // Create target
            targetMover = new SimpleMover
            {
                Position = size / 2,
                Modulate = Colors.LightBlue.WithAlpha(128)
            };
            AddChild(targetMover);

            // Create vehicle
            var vehicle = new Vehicle
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
