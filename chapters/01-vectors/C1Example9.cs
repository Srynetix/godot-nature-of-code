using Godot;
using Forces;

namespace Examples.Chapter1
{
    /// <summary>
    /// Example 1.9 - Velocity and random acceleration.
    /// </summary>
    /// Uses a custom SimpleMover with a custom UpdateAcceleration method.
    public class C1Example9 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 1.9:\n"
              + "Velocity & random accel.";
        }

        private class Mover : SimpleMover
        {
            protected override void UpdateAcceleration()
            {
                Acceleration = new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1));
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var mover = new Mover
            {
                Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y))
            };

            AddChild(mover);
        }
    }
}
