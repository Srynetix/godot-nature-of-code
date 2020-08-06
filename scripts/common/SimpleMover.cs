using Godot;

public class SimpleMover : Area2D
{
  public enum WrapModeEnum
  {
    Wrap,
    Bounce,
    None
  }

  public Vector2 Velocity = Vector2.Zero;
  public Vector2 Acceleration = Vector2.Zero;
  public float AngularVelocity = 0;
  public float AngularAcceleration = 0;
  public float BodySize = 20;
  public float MaxVelocity = 10;
  public float MaxAngularVelocity = 0.1f;
  public float Mass = 1;
  public WrapModeEnum WrapMode;

  public SimpleMover(WrapModeEnum wrapMode = WrapModeEnum.Wrap)
  {
    WrapMode = wrapMode;
  }

  public override void _Ready()
  {
    AddToGroup("movers");

    var collisionShape = new CollisionShape2D();
    var shape = new CircleShape2D();
    shape.Radius = BodySize;
    collisionShape.Shape = shape;
    AddChild(collisionShape);
  }

  public override void _Draw()
  {
    DrawCircle(Vector2.Zero, BodySize, Colors.LightBlue);
    DrawCircle(Vector2.Zero, BodySize - 2, Colors.White);
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

  public void ApplyForce(Vector2 force)
  {
    Acceleration += force / Mass;
  }

  public virtual void ApplyFriction(float coef)
  {
    var friction = (-Velocity).Normalized() * coef;
    ApplyForce(friction);
  }

  public virtual void ApplyDrag(float coef)
  {
    float speedSqr = Velocity.LengthSquared();
    float mag = coef * speedSqr;

    var drag = Velocity.Normalized() * mag * -1;
    ApplyForce(drag);
  }

  protected void ApplyMovement()
  {
    Velocity += Acceleration;
    Velocity = Velocity.Clamped(MaxVelocity);
    AngularVelocity += AngularAcceleration;
    AngularVelocity = Mathf.Clamp(AngularVelocity, -MaxAngularVelocity, MaxAngularVelocity);

    Position += Velocity;
    Rotation += AngularVelocity;

    Acceleration = Vector2.Zero;
    AngularAcceleration = 0;

    if (WrapMode == WrapModeEnum.Wrap)
    {
      WrapEdges();
    }
    else if (WrapMode == WrapModeEnum.Bounce)
    {
      BounceOnEdges();
    }
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

  protected void BounceOnEdges()
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

}
