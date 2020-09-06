using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Example 5.12 - Verlet Cluster.
  /// </summary>
  /// Uses VerletCluster.
  public class C5Example12 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 5.12:\n"
        + "Verlet Cluster\n\n"
        + "You can move points by touching them\n"
        + "If you drag points quick enough, links will break";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var physics = new VerletWorld();
      AddChild(physics);

      var center = size / 2;
      const int pointCount = 12;
      const int diameter = 128;

      new VerletCluster(
        physics,
        centerPosition: center,
        pointCount: pointCount,
        diameter: diameter,
        tearSensitivityFactor: 3,
        stiffness: 0.01f,
        drawPoints: true,
        pointRadius: 8f
      );
    }
  }
}
