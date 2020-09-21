using Godot;
using VerletPhysics;
using System.Collections.Generic;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.15 - Verlet Cluster Graph.
    /// </summary>
    /// Combines VerletClusters using VerletLink's MinimalDistance attribute.
    public class C5Exercise15 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.15:\n"
              + "Verlet Cluster Graph\n\n"
              + "You can move points by touching them";
        }

        private class VerletClusterGraph
        {
            public VerletClusterGraph(VerletWorld world, Vector2 centerPosition)
            {
                void linkClusters(VerletCluster cl1, VerletCluster cl2, float restingDistance, float minimalDistance, float linkStiffness)
                {
                    for (int i = 0; i < cl1.Points.Count; ++i)
                    {
                        for (int j = 0; j < cl2.Points.Count; ++j)
                        {
                            var a = cl1.Points[i];
                            var b = cl2.Points[j];

                            var link = world.CreateLink(a, b);
                            link.RestingDistance = restingDistance;
                            link.MinimalDistance = minimalDistance;
                            link.TearSensitivity = -1;
                            link.Modulate = Colors.LightCyan.WithAlpha(64);
                            link.Stiffness = linkStiffness;
                        }
                    }
                }

                const float stiffness = 0.01f;
                var clusters = new List<VerletCluster> {
          new VerletCluster(
            world,
            centerPosition + MathUtils.RandVector2(-5, 5, -5, 5),
            pointCount: 8,
            diameter: 50,
            tearSensitivityFactor: -1,
            stiffness: stiffness
          ),
          new VerletCluster(
            world,
            centerPosition + MathUtils.RandVector2(-5, 5, -5, 5),
            pointCount: 12,
            diameter: 75,
            tearSensitivityFactor: -1,
            stiffness: stiffness
          ),
          new VerletCluster(
            world,
            centerPosition + MathUtils.RandVector2(-5, 5, -5, 5),
            pointCount: 6,
            diameter: 25,
            tearSensitivityFactor: -1,
            stiffness: stiffness
          ),
          new VerletCluster(
            world,
            centerPosition + MathUtils.RandVector2(-5, 5, -5, 5),
            pointCount: 4,
            diameter: 50,
            tearSensitivityFactor: -1,
            stiffness: stiffness
          )
        };

                for (int i = 0; i < clusters.Count - 1; ++i)
                {
                    for (int j = i + 1; j < clusters.Count; ++j)
                    {
                        linkClusters(
                          clusters[i],
                          clusters[j],
                          restingDistance: 200,
                          minimalDistance: 10,
                          linkStiffness: 0.1f
                        );
                    }
                }
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var physics = new VerletWorld();
            AddChild(physics);

            new VerletClusterGraph(physics, size / 2);
        }
    }
}
