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
  public float MaxVelocity = 10;
  public float MaxAngularVelocity = 0.1f;
  public float Mass = 1;
  public WrapModeEnum WrapMode;
  public bool DisableForces = false;
  public SimpleMesh Mesh;

  public bool Drawing
  {
    get => Mesh.Drawing;
    set
    {
      Mesh.Drawing = value;
    }
  }

  public Vector2 BodySize
  {
    get => Mesh.BodySize;
    set
    {
      Mesh.BodySize = value;
    }
  }

  public float Radius
  {
    get => Mesh.BodySize.x / 2;
    set
    {
      Mesh.BodySize = new Vector2(value * 2, value * 2);
    }
  }

  public SimpleMover(WrapModeEnum wrapMode = WrapModeEnum.Wrap)
  {
    WrapMode = wrapMode;
    Mesh = new SimpleMesh();
  }

  public override void _Ready()
  {
    AddToGroup("movers");

    // Add collision shape
    var collisionShape = new CollisionShape2D();
    var shape = new CircleShape2D();
    shape.Radius = Radius;
    collisionShape.Shape = shape;
    AddChild(collisionShape);

    // Add mesh
    AddChild(Mesh);
  }

  public override void _Process(float delta)
  {
    if (!DisableForces)
    {
      UpdateAcceleration();
      ApplyMovement();
    }

    Update();
  }

  protected virtual void UpdateAcceleration()
  {
  }

  public virtual void ApplyForce(Vector2 force)
  {
    Acceleration += force / Mass;
  }

  public virtual void ApplyAngularForce(float force)
  {
    AngularAcceleration += force / Mass;
  }

  public virtual void ApplyFriction(float coef)
  {
    var friction = (-Velocity).Normalized() * coef;
    ApplyForce(friction);
  }

  public virtual void ApplyAngularFriction(float coef)
  {
    AngularAcceleration += -AngularVelocity * coef / 10f;
  }

  public virtual void ApplyDrag(float coef)
  {
    float speedSqr = Velocity.LengthSquared();
    float mag = coef * speedSqr;

    var drag = Velocity.Normalized() * mag * -1;
    ApplyForce(drag);
  }

  public virtual void ApplyDamping(float coef)
  {
    Velocity *= coef;
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
    var size = GetViewportRect().Size;

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
    var size = GetViewportRect().Size;
    var newPos = GlobalPosition;

    if (GlobalPosition.y < BodySize.y / 2)
    {
      Velocity.y *= -1;
      newPos.y = BodySize.y / 2;
    }
    else if (GlobalPosition.y > size.y - BodySize.y / 2)
    {
      Velocity.y *= -1;
      newPos.y = size.y - BodySize.y / 2;
    }

    if (GlobalPosition.x < BodySize.x / 2)
    {
      Velocity.x *= -1;
      newPos.x = BodySize.x / 2;
    }
    else if (GlobalPosition.x > size.x - BodySize.x / 2)
    {
      Velocity.x *= -1;
      newPos.x = size.x - BodySize.x / 2;
    }

    GlobalPosition = newPos;
  }
}