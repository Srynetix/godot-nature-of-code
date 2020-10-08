using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.16 - Verlet Attraction Combo.
    /// </summary>
    /// Uses two AttractionBehaviors, one for close objects, one for far objects.
    public class C5Exercise16 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.16\n"
              + "Verlet Attraction Combo\n\n"
              + "Attract particles far away, repulse particles at short distance\n"
              + "You can move points by touching them\n";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var physics = new VerletWorld();
            AddChild(physics);

            var attractor = physics.CreatePoint(size / 2, radius: 24, color: Colors.LightGoldenrod, mass: 1000);
            // Attract far away
            physics.AddBehavior(new AttractionBehavior(attractor, strength: 0.01f, radius: size.x));
            // Repulse when close
            physics.AddBehavior(new AttractionBehavior(attractor, strength: -0.1f, radius: 24 * 4));

            const int pointCount = 18;
            for (int i = 0; i < pointCount; ++i)
            {
                physics.CreatePoint(MathUtils.RandVector2(0, size.x, 0, size.y), radius: 16, mass: 10);
            }
        }
    }
}
