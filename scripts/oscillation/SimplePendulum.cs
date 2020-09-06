using Godot;
using Drawing;

namespace Oscillation
{
  /// <summary>
  /// Simple pendulum object.
  /// </summary>
  public class SimplePendulum : Node2D
  {
    /// <summary>Pendulum angular velocity</summary>
    public float AngularVelocity;

    /// <summary>Pendulum angular acceleration</summary>
    public float AngularAcceleration;

    /// <summary>Gravity value</summary>
    public float Gravity = 0.4f;

    /// <summary>Damping value</summary>
    public float Damping = 0.995f;

    /// <summary>Rope length</summary>
    public float RopeLength = 200;

    /// <summary>Ball radius</summary>
    public float Radius = 30;

    /// <summary>Pendulum angle</summary>
    public float Angle;

    /// <summary>Is user controllable?</summary>
    public bool UserControllable = true;

    private readonly SimpleLineSprite lineSprite;
    private readonly SimpleCircleSprite circleSprite;
    private bool touched = false;
    private int touchIndex = -1;
    private readonly Node2D children;

    /// <summary>
    /// Create a simple pendulum.
    /// </summary>
    public SimplePendulum()
    {
      circleSprite = new SimpleCircleSprite { Radius = 30 };
      lineSprite = new SimpleLineSprite { Width = 2 };
      children = new Node2D();
    }

    /// <summary>
    /// Add a child pendulum.
    /// </summary>
    /// <param name="pendulum">Pendulum instance</param>
    public void AddPendulumChild(SimplePendulum pendulum)
    {
      children.AddChild(pendulum);
    }

    public override void _Ready()
    {
      AddChild(lineSprite);
      AddChild(circleSprite);
      AddChild(children);

      lineSprite.PositionA = GlobalPosition;
      lineSprite.PositionB = GlobalPosition + GetBallPosition();
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
          Angle = -(touchAngle - (Mathf.Pi / 2));
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

        circleSprite.Modulate = Colors.LightBlue;
      }
      else
      {
        circleSprite.Modulate = Colors.LightGoldenrod;
      }

      // Update sprites
      var ballPos = GetBallPosition();
      circleSprite.GlobalPosition = GlobalPosition + ballPos;
      lineSprite.PositionA = GlobalPosition;
      lineSprite.PositionB = GlobalPosition + ballPos;

      // Update child pendulum positions
      foreach (Node2D child in children.GetChildren())
      {
        child.Position = ballPos;
      }
    }

    private Vector2 GetBallPosition()
    {
      return new Vector2(RopeLength * Mathf.Sin(Angle), RopeLength * Mathf.Cos(Angle));
    }
  }
}
