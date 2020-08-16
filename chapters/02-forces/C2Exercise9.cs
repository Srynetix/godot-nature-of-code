using System.Linq;

using Godot;

public class C2Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.9:\n"
      + "Distance Attraction";
  }

  public class Attractor : SimpleAttractor
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
      var bodySize = (float)GD.RandRange(5, 20);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
      var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
      mover.BodySize = new Vector2(bodySize, bodySize);
      mover.Mass = bodySize;
      mover.Position = new Vector2(xPos, yPos);

      var trail = new SimpleTrail();
      trail.Target = mover;

      AddChild(trail);
      AddChild(mover);
    }
  }
}
