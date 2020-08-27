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

  private SimpleLineSprite lineSprite;
  private SimpleCircleSprite circleSprite;
  private bool touched = false;
  private int touchIndex = -1;
  private Node2D children;

  public SimplePendulum()
  {
    circleSprite = new SimpleCircleSprite();
    circleSprite.Radius = 30;
    lineSprite = new SimpleLineSprite();
    lineSprite.Width = 2;
    children = new Node2D();
  }

  public override void _Ready()
  {
    AddChild(lineSprite);
    AddChild(circleSprite);
    AddChild(children);

    lineSprite.LineA = GlobalPosition;
    lineSprite.LineB = GlobalPosition + GetBallPosition();
  }

  public Vector2 GetBallPosition()
  {
    return new Vector2(RopeLength * Mathf.Sin(Angle), RopeLength * Mathf.Cos(Angle));
  }

  public void AddPendulumChild(SimplePendulum pendulum)
  {
    children.AddChild(pendulum);
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

      circleSprite.BaseColor = Colors.LightBlue;
    }
    else
    {
      circleSprite.BaseColor = Colors.LightGoldenrod;
    }

    // Update sprites
    var ballPos = GetBallPosition();
    circleSprite.GlobalPosition = GlobalPosition + ballPos;
    lineSprite.LineA = GlobalPosition;
    lineSprite.LineB = GlobalPosition + ballPos;

    // Update child pendulum positions
    foreach (Node2D child in children.GetChildren())
    {
      child.Position = ballPos;
    }
  }
}
