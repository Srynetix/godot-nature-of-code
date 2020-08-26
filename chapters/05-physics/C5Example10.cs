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

    var chain = physics.CreateChain(new Vector2[] {
      size / 4,
      size / 4 + new Vector2(20, 20),
      size / 4 + new Vector2(40, 40),
      size / 4 + new Vector2(60, 60),
      size / 4 + new Vector2(80, 80),
      size / 4 + new Vector2(100, 100)
    }, restingDistance: 50, tearSensitivity: 100, stiffness: 0.5f, drawIntermediatePoints: false);
  }
}
