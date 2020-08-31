using Godot;
using VerletPhysics;

public class C5Exercise13 : Node2D, IExample
{
  public string _Summary()
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

    var pointCount = new Vector2(12, 10);
    var separation = 30;
    var totalSize = pointCount * separation;
    var topLeftPosition = size / 2 - totalSize / 2;

    var cloth = new VerletCloth(
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
