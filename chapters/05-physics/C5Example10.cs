using Godot;
using VerletPhysics;

public class C5Example10 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 5.10:\n"
      + "Verlet Spring";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var physics = new VerletWorld();
    AddChild(physics);

    var p1 = physics.CreatePoint();
    p1.MoveToPosition(size / 2);
    p1.PinToCurrentPosition();

    var p2 = physics.CreatePoint();
    p2.MoveToPosition(size / 2 + new Vector2(20, 20));

    var p3 = physics.CreatePoint();
    p3.MoveToPosition(size / 2 + new Vector2(40, 40));

    var link1 = physics.CreateLink(p1, p2);
    var link2 = physics.CreateLink(p2, p3);
  }
}
