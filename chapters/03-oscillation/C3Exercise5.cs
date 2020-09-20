using Godot;
using Drawing;
using Forces;

namespace Examples.Chapter3
{
  /// <summary>
  /// Exercise 3.5 - Asteroids.
  /// </summary>
  /// Drive spaceship using arrow keys or VirtualControls.
  public class C3Exercise5 : Control, IExample
  {
    public string GetSummary()
    {
      return "Exercise 3.5:\n"
        + "Asteroids\n\n"
        + "On desktop, use left and right arrow keys to turn, then up arrow key to thrust.\n"
        + "On mobile, you can use the virtual controls.";
    }

    public class Spaceship : SimpleMover
    {
      /// <summary>Thrusting?</summary>
      protected bool thrusting;

      public Spaceship() : base(WrapModeEnum.Wrap)
      {
        MeshSize = new Vector2(20, 20);
        Mesh.MeshType = SimpleMesh.TypeEnum.Custom;
        Mesh.CustomDrawMethod = (pen) =>
        {
          // Body
          Vector2[] points = { new Vector2(-1, 1) * MeshSize, new Vector2(1, 1) * MeshSize, new Vector2(0, -1) * MeshSize };
          Color[] colors = { pen.Modulate, pen.Modulate, pen.Modulate };
          pen.DrawPolygon(points, colors);

          // Thrusters
          var thrusterColor = !thrusting ? Colors.White : Colors.Red;
          var thrusterSize = MeshSize / 3;
          pen.DrawRect(new Rect2((-MeshSize.x / 2) - (thrusterSize.x / 2), MeshSize.y, thrusterSize.x, thrusterSize.y), thrusterColor);
          pen.DrawRect(new Rect2((MeshSize.x / 2) - (thrusterSize.x / 2), MeshSize.y, thrusterSize.x, thrusterSize.y), thrusterColor);
        };
      }

      public void Accelerate(float amount)
      {
        Acceleration += Vector2.Right.Rotated(Rotation - (Mathf.Pi / 2)) * amount;
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

    /// <summary>Virtual controls</summary>
    protected VirtualControls controls;

    /// <summary>Spaceship</summary>
    protected Spaceship spaceship;

    public override void _Ready()
    {
      // Add virtual controls
      controls = new VirtualControls();
      AddChild(controls);

      spaceship = new Spaceship
      {
        Position = GetViewportRect().Size / 2
      };
      AddChild(spaceship);
    }

    public override void _Process(float delta)
    {
      const float accelFactor = 0.25f;
      const float turnFactor = 0.05f;

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
}
