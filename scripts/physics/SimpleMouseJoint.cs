using Godot;

namespace Physics
{
  public class SimpleMouseJoint : Node2D
  {
    public float Speed = 2;

    private bool _active = false;
    private RigidBody2D _parent;

    public override void _Ready()
    {
      _parent = (RigidBody2D)GetParent();
    }

    public virtual Vector2 ComputeTargetPosition()
    {
      return GetViewport().GetMousePosition();
    }

    public virtual bool IsActive()
    {
      return _active;
    }

    public override void _Process(float delta)
    {
      if (IsActive())
      {
        // Apply to parent object
        var r = ComputeTargetPosition() - _parent.Position;
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
      if (IsActive())
      {
        DrawLine(Vector2.Zero, (ComputeTargetPosition() - _parent.GlobalPosition).Rotated(-_parent.GlobalRotation), Colors.Gray, 2);
      }
    }
  }
}
