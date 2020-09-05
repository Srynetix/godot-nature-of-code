using System.Linq;
using Godot;
using Drawing;
using Forces;

namespace Examples.Chapter2
{
  /// <summary>
  /// Exercise 2.9 - Distance Attraction.
  /// </summary>
  /// Uses a SimpleAttractor with a custom Attract implementation to push items that are too close.
  public class C2Exercise9 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 2.9:\n"
        + "Distance Attraction";
    }

    private class Attractor : SimpleAttractor
    {
      public override Vector2 Attract(SimpleMover mover)
      {
        var force = Position - mover.Position;
        var length = Mathf.Clamp(force.Length(), 5, 50);
        var coef = 1;

        // Push if too close
        if (length < 45)
        {
          coef = -3;
        }

        float strength = (Gravitation * Mass * mover.Mass) / (length * length);
        return force.Normalized() * strength * coef;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var attractor1 = new Attractor();
      attractor1.Position = new Vector2(size.x / 4, size.y / 2);
      AddChild(attractor1);

      var attractor2 = new Attractor();
      attractor2.Position = new Vector2(size.x / 2, size.y / 2);
      AddChild(attractor2);

      var attractor3 = new Attractor();
      attractor3.Position = new Vector2(size.x - size.x / 4, size.y / 2);
      AddChild(attractor3);

      foreach (var x in Enumerable.Range(0, 10))
      {
        var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
        var bodySize = (float)GD.RandRange(20, 40);
        var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
        var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
        mover.MeshSize = new Vector2(bodySize, bodySize);
        mover.Mass = bodySize;
        mover.Position = new Vector2(xPos, yPos);

        var trail = new SimpleTrail();
        trail.Target = mover;

        AddChild(trail);
        AddChild(mover);
      }
    }
  }
}
