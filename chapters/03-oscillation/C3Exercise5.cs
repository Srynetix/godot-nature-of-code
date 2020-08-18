using Godot;

public class C3Exercise5 : Control, IExample
{
  public string _Summary()
  {
    return "Exercise 3.5:\n"
      + "Asteroids\n\n"
      + "On desktop, use left and right arrow keys to turn, then up arrow key to thrust.\n"
      + "On mobile, you can use the virtual controls.";
  }

  public class Spaceship : SimpleMover
  {
    protected bool thrusting = false;

    public Spaceship() : base(WrapModeEnum.Wrap)
    {
      BodySize = new Vector2(20, 20);
      Mesh.MeshType = SimpleMeshTypeEnum.Custom;
      Mesh.CustomDrawMethod = (pen) =>
      {
        var outlineVec = new Vector2(pen.OutlineWidth, pen.OutlineWidth);

        // Body
        Vector2[] points = { new Vector2(-1, 1) * BodySize, new Vector2(1, 1) * BodySize, new Vector2(0, -1) * BodySize };
        Color[] colors = { pen.OutlineColor, pen.OutlineColor, pen.OutlineColor };
        pen.DrawPolygon(points, colors);
        Vector2[] innerPoints = { new Vector2(-1, 1) * (BodySize - outlineVec), new Vector2(1, 1) * (BodySize - outlineVec), new Vector2(0, -1) * (BodySize - outlineVec) };

        Color[] innerColors = { pen.BaseColor, pen.BaseColor, pen.BaseColor };
        pen.DrawPolygon(innerPoints, innerColors);

        // Thrusters
        var thrusterColor = !thrusting ? Colors.White : Colors.Red;
        var thrusterSize = BodySize / 3;
        pen.DrawRect(new Rect2(-BodySize.x / 2 - thrusterSize.x / 2, BodySize.y, thrusterSize.x, thrusterSize.y), thrusterColor);
        pen.DrawRect(new Rect2(BodySize.x / 2 - thrusterSize.x / 2, BodySize.y, thrusterSize.x, thrusterSize.y), thrusterColor);
      };
    }

    public void Accelerate(float amount)
    {
      Acceleration += Vector2.Right.Rotated(Rotation - Mathf.Pi / 2) * amount;
    }

    public void Turn(float amount)
    {
      Rotation += amount;
    }

    protected override void UpdateAcceleration()
    {
      ApplyFriction(0.05f);
    }

    public override void _Process(float delta)
    {
      thrusting = (Acceleration.LengthSquared() > 0.01);

      base._Process(delta);
    }
  }

  protected VirtualControls controls;
  protected Spaceship spaceship;

  public override void _Ready()
  {
    // Add virtual controls
    controls = new VirtualControls();
    AddChild(controls);

    spaceship = new Spaceship();
    spaceship.Position = GetViewportRect().Size / 2;
    AddChild(spaceship);
  }

  public override void _Process(float delta)
  {
    var accelFactor = 0.25f;
    var turnFactor = 0.05f;

    if (controls.JoystickOutput.y < -0.5f || Input.IsActionPressed("ui_up"))
    {
      spaceship.Accelerate(accelFactor);
    }

    if (controls.JoystickOutput.x < -0.5f || Input.IsActionPressed("ui_left"))
    {
      spaceship.Turn(-turnFactor);
    }

    if (controls.JoystickOutput.x > 0.5f || Input.IsActionPressed("ui_right"))
    {
      spaceship.Turn(turnFactor);
    }
  }
}
