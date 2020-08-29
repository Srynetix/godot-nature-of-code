using Godot;
using VerletPhysics;

public class C5Example12 : Node2D, IExample
{
  public string _Summary() {
    return "Example 5.12:\n"
      + "Verlet Cluster\n\n"
      + "You can move points by touching them\n"
      + "If you drag points quick enough, links will break";
  }

  public override void _Ready() {
    var size = GetViewportRect().Size;
    var physics = new VerletWorld();
    physics.Gravity = Vector2.Zero;
    AddChild(physics);

    var center = size / 2;
    var pointCount = 12;
    var diameter = 128;

    physics.StartClusterBuilder(
      drawPoints: true,
      pointRadius: 8f
    )
      .GeneratePointsFromPosition(
        centerPosition: center,
        pointCount: pointCount,
        diameter: diameter
      ).Build(tearSensitivityFactor: 3, stiffness: 0.01f);
  }
}
