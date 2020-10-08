using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.17 - Verlet Attraction with Springs.
    /// </summary>
    /// Uses VerletCluster combined with AttractionBehavior.
    public class C5Exercise17 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.17\n"
              + "Verlet Attraction with Springs\n\n"
              + "You can move points by touching them\n";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var physics = new VerletWorld();
            AddChild(physics);

            // Attractor
            var attractor = physics.CreatePoint(size / 2, radius: 24, color: Colors.LightGoldenrod, mass: 1000);
            physics.AddBehavior(new AttractionBehavior(attractor, strength: 0.01f, radius: size.x));

            // Create clusters
            var cluster1 = new VerletCluster(
              physics,
              size / 4,
              pointCount: 8,
              diameter: 50
            );
            var cluster2 = new VerletCluster(
              physics,
              size * 0.75f,
              pointCount: 8,
              diameter: 50
            );

            // Hidden link to simulate collisions
            physics.CreateLink(cluster1.Points[0], cluster2.Points[0], minimalDistance: 50, visible: false);
        }
    }
}
