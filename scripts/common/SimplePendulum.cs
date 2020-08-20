using Godot;

public class SimplePendulum : Node2D
{
  public float AngularVelocity;
  public float AngularAcceleration;
  public float Gravity = 0.4f;
  public float Damping = 0.995f;
  public float RopeLength = 200;
  public float Radius = 30;
  public float Angle;
  public bool UserControllable = true;

  private bool touched = false;
  private int touchIndex = -1;

  public Vector2 GetBallPosition()
  {
    return new Vector2(RopeLength * Mathf.Sin(Angle), RopeLength * Mathf.Cos(Angle));
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed && touchIndex == -1)
      {
        if (eventScreenTouch.Position.DistanceTo(GlobalPosition + GetBallPosition()) < Radius * 2)
        {
          touchIndex = eventScreenTouch.Index;
          touched = true;
        }
      }

      if (!eventScreenTouch.Pressed && touchIndex == eventScreenTouch.Index)
      {
        touched = false;
        touchIndex = -1;
      }
    }

    else if (@event is InputEventScreenDrag eventScreenDrag)
    {
      if (touched)
      {
        // Compute angle from touch position
        var touchAngle = (eventScreenDrag.Position - GlobalPosition).Normalized().Angle();
        Angle = -(touchAngle - Mathf.Pi / 2);
      }
    }
  }

  public override void _Process(float delta)
  {
    if (!touched)
    {
      AngularAcceleration = -(Gravity / RopeLength) * Mathf.Sin(Angle);
      AngularVelocity += AngularAcceleration;
      Angle += AngularVelocity;

      AngularVelocity *= Damping;
    }

    // Update child pendulum positions
    foreach (Node2D child in GetChildren())
    {
      child.Position = GetBallPosition();
    }

    Update();
  }

  public override void _Draw()
  {
    var target = GetBallPosition();
    DrawLine(Vector2.Zero, target, Colors.LightGray);
    DrawCircle(target, Radius, Colors.LightBlue);

    var ballColor = touched ? Colors.LightGoldenrod : Colors.White;
    DrawCircle(target, Radius - 2, ballColor);
  }
}
