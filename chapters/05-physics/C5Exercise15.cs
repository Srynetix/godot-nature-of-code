using Godot;
using VerletPhysics;
using System.Collections.Generic;

public class C5Exercise15 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.15:\n"
      + "Verlet Cluster Graph\n\n"
      + "You can move points by touching them\n"
      + "If you drag points quick enough, links will break";
  }

  private class VerletClusterGraph
  {
    public VerletClusterGraph(VerletWorld world, Vector2 centerPosition)
    {
      void linkClusters(VerletCluster cl1, VerletCluster cl2, float restingDistance, float tearSensitivityFactor = -1, float linkStiffness = 1)
      {
        for (int i = 0; i < cl1.Points.Count; ++i)
        {
          for (int j = 0; j < cl2.Points.Count; ++j)
          {
            var a = cl1.Points[i];
            var b = cl2.Points[j];

            var link = world.CreateLink(a, b);
            link.RestingDistance = restingDistance;
            link.TearSensitivity = restingDistance * tearSensitivityFactor;
            link.Stiffness = linkStiffness;
          }
        }
      }

      var pointCount = 8;
      var diameter = 50;
      var stiffness = 0.01f;
      var clusterCount = 2;
      var clusters = new List<VerletCluster>();

      for (int i = 0; i < clusterCount; ++i)
      {
        clusters.Add(new VerletCluster(world, centerPosition + MathUtils.RandVector2(-5, 5, -5, 5), pointCount, diameter, stiffness: stiffness));
      }

      for (int i = 0; i < clusterCount - 1; ++i)
      {
        for (int j = i + 1; j < clusterCount; ++j)
        {
          linkClusters(clusters[i], clusters[j], diameter * 3, tearSensitivityFactor: 3, linkStiffness: 0.01f);
        }
      }
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var physics = new VerletWorld(gravity: Vector2.Zero);
    AddChild(physics);

    var graph = new VerletClusterGraph(physics, size / 2);
  }
}
