using Godot;

public class Mover : Node2D
{
  protected Vector2 velocity = Vector2.Zero;
  protected Vector2 acceleration = Vector2.One * 0.01f;
  protected float angularVelocity = 0;
  protected float angularAcceleration = 0.01f;
  protected float bodySize = 10;

  public override void _Ready()
  {
    bodySize = (float)GD.RandRange(5, 20);
  }

  public override void _Draw()
  {
    DrawRect(new Rect2(Vector2.Zero, Vector2.One * (bodySize + 1)), Colors.Black);
    DrawRect(new Rect2(Vector2.One, Vector2.One * (bodySize - 1)), Colors.LightBlue);
  }

  public override void _Process(float delta)
  {
    acceleration += new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1)) * delta;

    velocity += acceleration;
    velocity.x = Mathf.Clamp(velocity.x, -1.0f, 1.0f);
    velocity.y = Mathf.Clamp(velocity.y, -1.0f, 1.0f);

    Position += velocity;

    angularAcceleration = acceleration.x / 10.0f;
    angularVelocity += angularAcceleration;
    angularVelocity = Mathf.Clamp(angularVelocity, -0.1f, 0.1f);
    Rotation += angularVelocity;

    WrapEdges();
    Update();
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
