using Godot;

public class SimpleMouseJoint : Node2D
{
  public float Speed = 2;

  private bool _active = false;
  private RigidBody2D _parent;

  public override void _Ready()
  {
    _parent = (RigidBody2D)GetParent();
  }

  public override void _Process(float delta)
  {
    if (_active)
    {
      // Apply to parent object
      var r = GetViewport().GetMousePosition() - _parent.Position;
      _parent.LinearVelocity = r * Speed;
    }

    Update();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        _active = true;
      }
      else
      {
        _active = false;
      }
    }
  }

  public override void _Draw()
  {
    if (_active)
    {
      DrawLine(Vector2.Zero, (GetViewport().GetMousePosition() - _parent.GlobalPosition).Rotated(-_parent.GlobalRotation), Colors.Gray, 2);
    }
  }
}
