using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  public class VerletPoint : Node2D
  {
    private const int mouseDetectionRadiusThreshold = 10;

    public float Radius = 15f;
    public Color BaseColor = Colors.LightBlue;
    public float Mass = 1;
    public float Bounce = 0.9f;
    public float Friction = 0.99f;
    public float GravityScale = 1;
    public Vector2 Acceleration;
    public bool Drawing = true;

    private bool pinned = false;
    private Vector2 pinPosition;
    private Vector2 prevPosition;
    private VerletWorld world;
    private List<VerletLink> links;
    private bool touched = false;
    private int touchIndex = -1;

    public VerletPoint(VerletWorld world)
    {
      this.world = world;
      links = new List<VerletLink>();
    }

    public void MoveToPosition(Vector2 position)
    {
      GlobalPosition = position;
      prevPosition = GlobalPosition;
    }
    public override void _Process(float delta)
    {
      Update();
    }

    public override void _Draw()
    {
      if (Drawing)
      {
        var color = BaseColor;
        if (touched)
        {
          color = Colors.LightGoldenrod;
        }

        DrawCircle(Vector2.Zero, Radius, color);
      }
    }

    private void FixVelocity(Vector2 velocity)
    {
      prevPosition = GlobalPosition + velocity;
    }

    private Vector2 ComputeVelocity()
    {
      return (GlobalPosition - prevPosition);
    }

    private void ApplyGravity(Vector2 gravity)
    {
      ApplyForce(gravity * GravityScale);
    }

    public void ApplyForce(Vector2 force)
    {
      Acceleration += force / Mass;
    }

    public void AddLink(VerletLink link)
    {
      links.Add(link);
    }

    public void RemoveLink(VerletLink link)
    {
      links.Remove(link);
    }

    public void ApplyMovement(Vector2 gravity, float delta)
    {
      if (!touched)
      {
        // Update acceleration
        ApplyGravity(gravity);
      }

      var velocity = ComputeVelocity() * Friction;
      var nextPosition = GlobalPosition + velocity + Acceleration * delta;

      prevPosition = GlobalPosition;
      GlobalPosition = nextPosition;

      Acceleration = Vector2.Zero;
    }

    public void ApplyConstraints(Vector2 worldSize)
    {
      foreach (VerletLink link in links)
      {
        link.Constraint();
      }

      ConstraintPositionInVector(worldSize);
      ConstraintPinning();
    }

    public void PinToCurrentPosition()
    {
      pinned = true;
      pinPosition = GlobalPosition;
    }

    public void PinToPosition(Vector2 position)
    {
      pinned = true;
      pinPosition = position;
    }

    public void Unpin()
    {
      pinned = false;
    }

    private void ConstraintPinning()
    {
      if (pinned)
      {
        GlobalPosition = pinPosition;
      }
    }

    private void ConstraintPositionInVector(Vector2 worldSize)
    {
      var size = worldSize;
      var newPos = GlobalPosition;
      var velocity = ComputeVelocity() * Bounce;
      var bodySize = new Vector2(Radius / 2, Radius / 2);

      if (GlobalPosition.y < bodySize.y / 2)
      {
        GlobalPosition = new Vector2(GlobalPosition.x, bodySize.y / 2);
        velocity.x *= -1;
        FixVelocity(velocity);
      }
      else if (GlobalPosition.y > size.y - bodySize.y / 2)
      {
        GlobalPosition = new Vector2(GlobalPosition.x, size.y - bodySize.y / 2);
        velocity.x *= -1;
        FixVelocity(velocity);
      }

      if (GlobalPosition.x < bodySize.x / 2)
      {
        GlobalPosition = new Vector2(bodySize.x / 2, GlobalPosition.y);
        velocity.y *= -1;
        FixVelocity(velocity);
      }
      else if (GlobalPosition.x > size.x - bodySize.x / 2)
      {
        GlobalPosition = new Vector2(size.x - bodySize.x / 2, GlobalPosition.y);
        velocity.y *= -1;
        FixVelocity(velocity);
      }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
      if (@event is InputEventScreenTouch eventScreenTouch)
      {
        if (eventScreenTouch.Pressed && touchIndex == -1)
        {
          if (GlobalPosition.DistanceTo(eventScreenTouch.Position) < Radius + mouseDetectionRadiusThreshold)
          {
            touchIndex = eventScreenTouch.Index;
            touched = true;
          }
        }

        else if (!eventScreenTouch.Pressed && eventScreenTouch.Index == touchIndex)
        {
          touchIndex = -1;
          touched = false;
        }
      }

      else if (@event is InputEventScreenDrag eventScreenDrag)
      {
        if (eventScreenDrag.Index == touchIndex && touched)
        {
          prevPosition = GlobalPosition - eventScreenDrag.Relative;
        }
      }
    }
  }
}
