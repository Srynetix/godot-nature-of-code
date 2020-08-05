using Godot;

public class C2Example1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.1:\n"
      + "Forces";
  }

  public class Mover : Node2D
  {
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float MaxVelocity = 10.0f;
    public float BodySize = 20;
    public float Mass = 10;

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
      var size = GetViewport().Size;
      Position = size / 2;
    }

    public override void _Process(float delta)
    {
      var wind = new Vector2(0.1f, 0);
      var gravity = new Vector2(0, 0.9f);

      ApplyForce(wind);
      ApplyForce(gravity);

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
    AddChild(new Mover());
  }
}
