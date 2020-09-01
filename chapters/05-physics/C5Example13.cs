using Godot;
using VerletPhysics;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Example 5.13 - Verlet Attraction/Repulsion.
    /// </summary>
    /// Uses VerletWorld and AttractionBehavior.
    public class C5Example13 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 5.13\n"
          + "Verlet Attraction/Repulsion\n\n"
          + "You can move points by touching them\n";
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var physics = new VerletWorld();
        AddChild(physics);

        var attractor = physics.CreatePoint(size / 2, radius: 24, color: Colors.LightGoldenrod, mass: 1000);
        physics.AddBehavior(new AttractionBehavior(attractor, strength: 0.01f, radius: size.x));

        var pointCount = 18;

        for (int i = 0; i < pointCount; ++i)
        {
          var pt = physics.CreatePoint(MathUtils.RandVector2(0, size.x, 0, size.y), radius: 16, mass: 10);
          physics.AddBehavior(new AttractionBehavior(pt, strength: -0.1f, radius: pt.Radius * 4));
        }
      }
    }
  }
}
