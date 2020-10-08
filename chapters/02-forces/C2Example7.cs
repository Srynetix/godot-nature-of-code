using System.Linq;
using Godot;
using Forces;

namespace Examples.Chapter2
{
    /// <summary>
    /// Example 2.7 - Attraction with many movers.
    /// </summary>
    /// Same principle than Example 2.6 but with many movers.
    public class C2Example7 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 2.7:\n"
              + "Attraction with many Movers";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var attractor = new SimpleAttractor
            {
                Gravitation = 0.5f,
                Position = size / 2
            };
            AddChild(attractor);

            foreach (var _ in Enumerable.Range(0, 10))
            {
                var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
                var bodySize = (float)GD.RandRange(20, 40);
                mover.MeshSize = new Vector2(bodySize, bodySize);
                mover.Mass = bodySize;
                var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
                var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
                mover.Position = new Vector2(xPos, yPos);
                AddChild(mover);
            }
        }
    }
}
