using Godot;

public class C3Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.2:\n"
      + "Cannon";
  }

  public class Cannon : Node2D
  {
    public float BasisSize = 20;
    public float MovementSpeed = 1;

    private float t = 0;

    public override void _Draw()
    {
      var bodyWidth = BasisSize * 2;
      var bodyLength = bodyWidth * 2;

      // Basis
      DrawRect(new Rect2(0, -bodyWidth / 2, bodyLength, bodyWidth), Colors.Purple);
      DrawRect(new Rect2(2, -(bodyWidth - 4) / 2, bodyLength - 4, bodyWidth - 4), Colors.White);
      DrawCircle(Vector2.Zero, BasisSize, Colors.LightBlue);
      DrawCircle(Vector2.Zero, BasisSize - 2, Colors.White);
    }

    public Vector2 GetSpawnPoint()
    {
      var spawnPointLength = BasisSize * 4 - 25;
      return new Vector2(spawnPointLength, 0).Rotated(Rotation) + GlobalPosition;
    }

    public override void _Process(float delta)
    {
      Rotation = Mathf.Deg2Rad(-45 + Mathf.Sin(t) * 45);
      t += delta * MovementSpeed;
    }

    public void Fire()
    {
      var spawnPoint = GetSpawnPoint();

      var proj = new Projectile();
      proj.Position = spawnPoint;
      proj.Rotation = Rotation;
      GetParent().AddChild(proj);
    }
  }

  public class Projectile : SimpleMover
  {
    public bool Fired = false;

    public Projectile() : base(WrapModeEnum.Bounce)
    {
      MaxVelocity = 100f;
      MaxAngularVelocity = 20f;
    }

    protected override void UpdateAcceleration()
    {
      if (!Fired)
      {
        // Boom
        var direction = Vector2.Right.Rotated(Rotation).Normalized();
        ApplyForce(direction * 40f);
        Fired = true;
      }

      AngularAcceleration = Acceleration.x / 10.0f;

      // Gravity
      ApplyForce(new Vector2(0, 0.981f));
      ApplyFriction(0.25f);
      ApplyAngularFriction(0.25f);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(-Radius / 2, -Radius / 2, Radius, Radius), Colors.LightBlue);
      DrawRect(new Rect2(-Radius / 2 + 2, -Radius / 2 + 2, Radius - 4, Radius - 4), Colors.White);
    }
  }

  private Timer timer;
  private Cannon cannon;

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var offset = 50;
    var fireTime = 1f;

    timer = new Timer();
    timer.WaitTime = fireTime;
    timer.Autostart = true;
    AddChild(timer);

    timer.Connect("timeout", this, nameof(CannonFire));

    cannon = new Cannon();
    cannon.Position = new Vector2(offset, size.y - offset);
    AddChild(cannon);
  }

  public void CannonFire()
  {
    cannon.Fire();
  }
}
