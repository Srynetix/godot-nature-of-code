using Godot;

public class SimpleMover : Node2D
{
  public Vector2 Velocity = Vector2.Zero;
  public Vector2 Acceleration = Vector2.Zero;
  public float BodySize = 20;
  public float TopSpeed = 10;

  public override void _Draw()
  {
    DrawCircle(Vector2.Zero, BodySize, Colors.Black);
    DrawCircle(Vector2.Zero, BodySize - 2, Colors.LightGray);
  }

  public override void _Process(float delta)
  {
    UpdateAcceleration();
    ApplyMovement();
    Update();
  }

  protected virtual void UpdateAcceleration()
  {
  }

  protected void ApplyMovement()
  {
    Velocity += Acceleration;
    Velocity = Velocity.Clamped(TopSpeed);
    Position += Velocity;

    WrapEdges();
  }

  protected void WrapEdges()
  {
    var size = GetViewport().Size;

    if (GlobalPosition.x > size.x)
    {
      GlobalPosition = new Vector2(0, GlobalPosition.y);
    }
    else if (GlobalPosition.x < 0)
    {
      GlobalPosition = new Vector2(size.x, GlobalPosition.y);
    }

    if (GlobalPosition.y > size.y)
    {
      GlobalPosition = new Vector2(GlobalPosition.x, 0);
    }
    else if (GlobalPosition.y < 0)
    {
      GlobalPosition = new Vector2(GlobalPosition.x, size.y);
    }
  }
}
