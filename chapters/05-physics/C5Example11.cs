using Godot;
using VerletPhysics;

public class C5Example11 : Node2D, IExample
{
  public string _Summary()
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
    AddChild(physics);

    new VerletChainBuilder(physics)
      .AddPointAtPosition(x: size.x / 2, y: 0)
      .AddPointsWithOffset(pointCount: 8, x: 10, y: 10)
      .AddPointWithOffset(x: 10, y: 10, configurator: (point) =>
      {
        point.Radius = 30;
      })
      .Build(stiffness: 0.5f);
  }
}
