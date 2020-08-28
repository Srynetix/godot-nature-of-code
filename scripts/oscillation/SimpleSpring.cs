using Godot;

public class SimpleSpring : Node2D
{
  // Spring anchor is the node position
  // Manage children as targets

  public float Length = 100;
  public float K = 0.2f;
  public float MinLength = 50;
  public float MaxLength = 150;

  private SimpleMover currentMover = null;
  private int touchIndex = -1;
  private bool touched = false;
  private SimpleLineSprite lineSprite;

  public SimpleSpring()
  {
    lineSprite = new SimpleLineSprite();
    lineSprite.Width = 2;
  }

  public override void _Ready()
  {
    lineSprite.PositionA = GlobalPosition;
    lineSprite.PositionB = GlobalPosition;
    AddChild(lineSprite);
  }

  public virtual Vector2 ComputeForce(SimpleMover mover)
  {
    if (touched)
    {
      return Vector2.Zero;
    }

    var force = mover.Position;
    var length = force.Length();
    var stretch = length - Length;

    return force.Normalized() * -K * stretch;
  }

  public void ConstrainLength()
  {
    if (currentMover == null)
    {
      return;
    }

    var dir = currentMover.Position;
    var d = dir.Length();

    if (d < MinLength)
    {
      currentMover.Position = dir.Normalized() * MinLength;
      currentMover.Velocity = Vector2.Zero;
    }

    else if (d > MaxLength)
    {
      currentMover.Position = dir.Normalized() * MaxLength;
      currentMover.Velocity = Vector2.Zero;
    }
  }

  // public override void _Draw()
  // {
  //   if (currentMover != null)
  //   {
  //     var color = touched ? Colors.LightGoldenrod : Colors.LightGray;
  //     DrawLine(Vector2.Zero, currentMover.Position, color, 2, true);
  //   }
  // }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (currentMover == null)
    {
      return;
    }

    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed && touchIndex == -1)
      {
        if (eventScreenTouch.Position.DistanceTo(currentMover.GlobalPosition) < currentMover.Radius * 2)
        {
          touchIndex = eventScreenTouch.Index;
          currentMover.DisableForces = true;
          touched = true;
        }
      }

      if (!eventScreenTouch.Pressed && touchIndex == eventScreenTouch.Index)
      {
        currentMover.DisableForces = false;
        touched = false;
        touchIndex = -1;
      }
    }

    else if (@event is InputEventScreenDrag eventScreenDrag)
    {
      if (touched)
      {
        currentMover.GlobalPosition = eventScreenDrag.Position;
      }
    }
  }

  public override void _Process(float delta)
  {
    if (currentMover != null)
    {
      currentMover.ApplyForce(ComputeForce(currentMover));
    }

    ConstrainLength();

    if (currentMover != null)
    {
      // Update line
      var color = touched ? Colors.LightGoldenrod : Colors.LightGray;
      lineSprite.PositionA = GlobalPosition;
      lineSprite.PositionB = currentMover.GlobalPosition;
      lineSprite.Modulate = color;
    }
  }

  public void SetMover(SimpleMover mover, Vector2 initialPosition)
  {
    if (currentMover != null)
    {
      RemoveChild(currentMover);
      currentMover.QueueFree();
    }

    currentMover = mover;
    AddChild(currentMover);

    // Set position
    currentMover.Position = initialPosition;
  }
}
