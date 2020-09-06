using Godot;
using Drawing;

namespace Forces
{
  /// <summary>
  /// Generic force-driven object.
  /// </summary>
  public class SimpleMover : Area2D
  {
    /// <summary>
    /// Wrapping mode enum.
    /// </summary>
    public enum WrapModeEnum
    {
      /// <summary>Wrap on edges</summary>
      Wrap,

      /// <summary>Bounce on edges</summary>
      Bounce,

      /// <summary>No edge limit</summary>
      None
    }

    /// <summary>Current velocity</summary>
    public Vector2 Velocity = Vector2.Zero;

    /// <summary>Current acceleration</summary>
    public Vector2 Acceleration = Vector2.Zero;

    /// <summary>Current angular velocity</summary>
    public float AngularVelocity = 0;

    /// <summary>Current angular acceleration</summary>
    public float AngularAcceleration = 0;

    /// <summary>Max velocity</summary>
    public float MaxVelocity = 10;

    /// <summary>Max angular velocity</summary>
    public float MaxAngularVelocity = 0.1f;

    /// <summary>Mass</summary>
    public float Mass = 1;

    /// <summary>Wrapping mode</summary>
    public WrapModeEnum WrapMode;

    /// <summary>Disable forces</summary>
    public bool DisableForces = false;

    /// <summary>Synchronize rotation on velocity</summary>
    public bool SyncRotationOnVelocity = false;

    /// <summary>Mesh</summary>
    public SimpleMesh Mesh;

    /// <summary>Draw mesh</summary>
    public bool Drawing
    {
      get => Mesh.Visible;
      set
      {
        Mesh.Visible = value;
      }
    }

    /// <summary>Mesh size</summary>
    public Vector2 MeshSize
    {
      get => Mesh.MeshSize;
      set
      {
        Mesh.MeshSize = value;
      }
    }

    /// <summary>Mesh radius</summary>
    public float Radius
    {
      get => Mesh.MeshSize.x / 2;
      set
      {
        Mesh.MeshSize = new Vector2(value * 2, value * 2);
      }
    }

    protected CollisionShape2D collisionShape2D;

    /// <summary>
    /// Create a simple wrapping mover with a circle mesh.
    /// </summary>
    public SimpleMover(WrapModeEnum wrapMode = WrapModeEnum.Wrap)
    {
      WrapMode = wrapMode;
      Mesh = new SimpleMesh();
      Name = "SimpleMover";

      collisionShape2D = new CollisionShape2D { Name = "CollisionShape2D" };
    }

    /// <summary>
    /// Apply force on mover.
    /// <summary>
    /// <param name="force">Force vector</param>
    public virtual void ApplyForce(Vector2 force)
    {
      Acceleration += force / Mass;
    }

    /// <summary>
    /// Apply angular force on mover.
    /// </summary>
    /// <param name="force">Force angle in radians</param>
    public virtual void ApplyAngularForce(float force)
    {
      AngularAcceleration += force / Mass;
    }

    /// <summary>
    /// Apply friction on mover.
    /// </summary>
    /// <param name="coef">Friction coefficient</param>
    public virtual void ApplyFriction(float coef)
    {
      var friction = (-Velocity).Normalized() * coef;
      ApplyForce(friction);
    }

    /// <summary>
    /// Apply angular friction on mover.
    /// </summary>
    /// <param name="coef">Friction coefficient</param>
    public virtual void ApplyAngularFriction(float coef)
    {
      AngularAcceleration += -AngularVelocity * coef / 10f;
    }

    /// <summary>
    /// Apply drag on mover.
    /// </summary>
    /// <param name="coef">Drag coefficient</param>
    public virtual void ApplyDrag(float coef)
    {
      float speedSqr = Velocity.LengthSquared();
      float mag = coef * speedSqr;

      var drag = Velocity.Normalized() * mag * -1;
      ApplyForce(drag);
    }

    /// <summary>
    /// Apply damping on mover.
    /// </summary>
    /// <param name="coef">Damping coefficient</param>
    public virtual void ApplyDamping(float coef)
    {
      Velocity *= coef;
    }

    /// <summary>
    /// Update acceleration value.
    /// </summary>
    protected virtual void UpdateAcceleration() { }

    public override void _Ready()
    {
      AddToGroup("movers");

      // Add collision shape
      collisionShape2D.Shape = new CircleShape2D { Radius = Radius };
      AddChild(collisionShape2D);

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

      if (SyncRotationOnVelocity)
      {
        Rotation = Mathf.Atan2(Velocity.y, Velocity.x);
      }
    }

    private void ApplyMovement()
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

    private void WrapEdges()
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

    private void BounceOnEdges()
    {
      var size = GetViewportRect().Size;
      var newPos = GlobalPosition;

      if (GlobalPosition.y < MeshSize.y / 2)
      {
        Velocity.y *= -1;
        newPos.y = MeshSize.y / 2;
      }
      else if (GlobalPosition.y > size.y - (MeshSize.y / 2))
      {
        Velocity.y *= -1;
        newPos.y = size.y - (MeshSize.y / 2);
      }

      if (GlobalPosition.x < MeshSize.x / 2)
      {
        Velocity.x *= -1;
        newPos.x = MeshSize.x / 2;
      }
      else if (GlobalPosition.x > size.x - (MeshSize.x / 2))
      {
        Velocity.x *= -1;
        newPos.x = size.x - (MeshSize.x / 2);
      }

      GlobalPosition = newPos;
    }
  }
}
