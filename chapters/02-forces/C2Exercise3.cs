using System.Linq;

using Godot;

public class C2Exercise3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.3:\n"
      + "Instead of objects bouncing off the edge of the wall, create an example in which an invisible force pushes back on the objects to keep them in the window.\n"
      + "Can you weight the force according to how far the object is from an edge—i.e., the closer it is, the stronger the force?";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    public Vector2 ComputeWindForce()
    {
      var size = GetViewport().Size;
      var pos = Position;
      var output = Vector2.Zero;
      var limit = BodySize * 8;

      // Push left
      if (Position.x > size.x - limit)
      {
        var force = limit * 2 - (size.x - Position.x);
        output.x = -force * 0.01f;
      }

      // Push right
      else if (Position.x < limit)
      {
        var force = limit * 2 - Position.x;
        output.x = force * 0.01f;
      }

      else
      {
        output.x = 0.1f;
      }

      return output;
    }

    protected override void UpdateAcceleration()
    {
      var gravity = new Vector2(0, 0.9f);

      ApplyForce(ComputeWindForce());
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      var bodySize = (float)GD.RandRange(5, 20);
      var size = GetViewport().Size;
      var xPos = (float)GD.RandRange(bodySize * 4, size.x - bodySize * 4);
      mover.BodySize = bodySize;
      mover.Mass = (float)GD.RandRange(5, 10);
      mover.Position = new Vector2(xPos, size.y / 2);
      AddChild(mover);
    }
  }
}
