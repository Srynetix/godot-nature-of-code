using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.13 - Verlet Cloth.
    /// </summary>
    /// Uses VerletCloth.
    public class C5Exercise13 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.13:\n"
              + "Verlet Cloth\n\n"
              + "You can move points by touching them\n"
              + "If you drag points quick enough, links will break";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var physics = new VerletWorld();
            physics.AddBehavior(new GravityBehavior());
            AddChild(physics);

            const int separation = 30;
            var pointCount = new Vector2(12, 10);
            var totalSize = pointCount * separation;
            var topLeftPosition = (size / 2) - (totalSize / 2);

            new VerletCloth(
              physics,
              topLeftPosition: topLeftPosition,
              pointCount: pointCount,
              separation: separation,
              pinMode: VerletCloth.PinModeEnum.TopCorners,
              tearSensitivityFactor: 2f,
              stiffness: 1f,
              drawPoints: true,
              pointRadius: 8f
            );
        }
    }
}
