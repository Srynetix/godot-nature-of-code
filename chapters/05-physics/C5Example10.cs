using Godot;
using VerletPhysics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Example 5.10 - Verlet Spring.
  /// </summary>
  /// Uses a custom verlet physics implementation (uses VerletWorld and VerletChainBuilder).
  public class C5Example10 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 5.10:\n"
        + "Verlet Spring\n\n"
        + "Verlet physics is not available in Godot, so it has been implemented from scratch.\n"
        + "You can move points by touching them.";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var physics = new VerletWorld();
      physics.AddBehavior(new GravityBehavior());
      AddChild(physics);

      new VerletChainBuilder(physics)
        .AddPointAtPosition(new Vector2(size.x / 2, 0))
        .AddPointAtPosition(size / 2 + new Vector2(80, 0))
        .Build(restingDistance: size.y / 2, tearSensitivity: -1, stiffness: 0.5f);
    }
  }
}
