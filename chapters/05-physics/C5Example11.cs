using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Example 5.11 - Verlet Soft Pendulum.
    /// </summary>
    /// Uses VerletChainBuilder with hidden VerletPoint to visually simulate a chain.
    public class C5Example11 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 5.11:\n"
              + "Verlet Soft Pendulum\n\n"
              + "You can move points by touching them\n"
              + "If you drag points quick enough, links will break";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var physics = new VerletWorld();
            physics.AddBehavior(new GravityBehavior());
            AddChild(physics);

            new VerletChainBuilder(physics)
              .AddPointAtPosition(x: size.x / 2, y: 0)
              .AddPointsWithOffset(pointCount: 8, x: 10, y: 10)
              .AddPointWithOffset(x: 10, y: 10, configurator: (point) => point.Radius = 30)
              .Build(stiffness: 0.5f);
        }
    }
}
