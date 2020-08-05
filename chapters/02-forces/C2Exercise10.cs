using System.Linq;

using Godot;

public class C2Exercise10 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.10:\n"
      + "Change the attraction force in Example 2.8 to a repulsion force.\n"
      + "Can you create an example in which all of the Mover objects are attracted to the mouse, but repel each other?";
  }

  public class Mover : Node2D
  {
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float MaxVelocity = 5;
    public float BodySize = 20;
    public float Mass = 10;
    public float Gravitation = 1.0f;

    public Vector2 Attract(Mover mover)
    {
      var force = Position - mover.Position;
      var length = Mathf.Clamp(force.Length(), 5, 25);
      float strength = (Gravitation * Mass * mover.Mass) / (length * length);
      return force.Normalized() * strength;
    }

    public Vector2 AttractToMouse(Mover mover)
    {
      var mousePos = GetGlobalMousePosition();
      var mouseGravitation = 1;
      var mouseMass = 100;

      var force = mousePos - mover.Position;
      var length = Mathf.Clamp(force.Length(), 5, 25);
      float strength = (mouseGravitation * mouseMass * mover.Mass) / (length * length);
      return force.Normalized() * strength;
    }

    public void ApplyForce(Vector2 force)
    {
      Acceleration += force / Mass;
    }

    public void Move()
    {
      Velocity = (Velocity + Acceleration).Clamped(MaxVelocity);
      Position += Velocity;
      Acceleration = Vector2.Zero;

      BounceOnEdges();
    }

    public void BounceOnEdges()
    {
      var size = GetViewport().Size;
      var newPos = Position;

      if (Position.y < BodySize / 2)
      {
        Velocity.y *= -1;
        newPos.y = BodySize / 2;
      }
      else if (Position.y > size.y - BodySize / 2)
      {
        Velocity.y *= -1;
        newPos.y = size.y - BodySize / 2;
      }

      if (Position.x < BodySize / 2)
      {
        Velocity.x *= -1;
        newPos.x = BodySize / 2;
      }
      else if (Position.x > size.x - BodySize / 2)
      {
        Velocity.x *= -1;
        newPos.x = size.x - BodySize / 2;
      }

      Position = newPos;
    }

    public override void _Ready()
    {
      AddToGroup("attractors");

      var size = GetViewport().Size;
      var xPos = (float)GD.RandRange(BodySize, size.x - BodySize);
      var yPos = (float)GD.RandRange(BodySize, size.y - BodySize);
      Position = new Vector2(xPos, yPos);
    }

    public override void _Process(float delta)
    {
      // For each attractor
      foreach (var n in GetTree().GetNodesInGroup("attractors"))
      {
        if (n != this)
        {
          var attractor = (Mover)n;
          var force = attractor.Attract(this);
          ApplyForce(-force);
        }
      }

      ApplyForce(AttractToMouse(this));

      Move();
    }

    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, BodySize, Colors.LightBlue.WithAlpha(200));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.White.WithAlpha(200));
    }
  }

  public override void _Ready()
  {
    var size = GetViewport().Size;

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = mover.BodySize;
      AddChild(mover);
    }
  }
}
