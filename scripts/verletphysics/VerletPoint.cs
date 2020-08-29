using Godot;
using System.Collections.Generic;
using Drawing;

namespace VerletPhysics
{
  /// <summary>
  /// Verlet point.
  /// </summary>
  public class VerletPoint : SimpleCircleSprite
  {
    /// <summary>Point mass</summary>
    public float Mass = 1;
    /// <summary>Bounce coefficient</summary>
    public float Bounce = 0.9f;
    /// <summary>Friction coefficient</summary>
    public float Friction = 0.99f;
    /// <summary>Gravity scale</summary>
    public float GravityScale = 1;
    /// <summary>Current acceleration</summary>
    public Vector2 Acceleration;

    private const int mouseDetectionRadiusThreshold = 10;

    private bool pinned = false;
    private Vector2 pinPosition;
    private Vector2 prevPosition;
    private VerletWorld world;
    private List<VerletLink> links;
    private bool touched = false;
    private int touchIndex = -1;

    /// <summary>
    /// Create a default verlet point of 15px radius.
    /// </summary>
    /// <param name="world">Verlet world</param>
    public VerletPoint(VerletWorld world)
    {
      Radius = 15f;
      Modulate = Colors.LightBlue;

      this.world = world;
      links = new List<VerletLink>();
    }

    /// <summary>
    /// Move point to position.
    /// </summary>
    /// <param name="position">Target position</param>
    public void MoveToPosition(Vector2 position)
    {
      GlobalPosition = position;
      prevPosition = GlobalPosition;
    }

    /// <summary>
    /// Apply force on point.
    /// </summary>
    /// <param name="force">Force</param>
    public void ApplyForce(Vector2 force)
    {
      Acceleration += force / Mass;
    }

    /// <summary>
    /// Associate verlet link with current point.
    /// </summary>
    /// <param name="link">Verlet link</param>
    public void AddLink(VerletLink link)
    {
      links.Add(link);
    }

    /// <summary>
    /// Deassociate verlet link from current point.
    /// </summary>
    /// <param name="link">Verlet link</param>
    public void RemoveLink(VerletLink link)
    {
      links.Remove(link);
    }

    /// <summary>
    /// Update movement using current forces and gravity.
    /// </summary>
    /// <param name="gravity">Gravity force</param>
    /// <param name="delta">Delta time</param>
    public void UpdateMovement(Vector2 gravity, float delta)
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

    /// <summary>
    /// Apply constraints on point.
    /// </summary>
    /// <param name="worldSize">World size</param>
    public void Constraint(Vector2 worldSize)
    {
      foreach (VerletLink link in links)
      {
        link.Constraint();
      }

      ConstraintPositionInSize(worldSize);
      ConstraintPinning();
    }

    /// <summary>
    /// Pin point to its current position.
    /// </summary>
    public void PinToCurrentPosition()
    {
      pinned = true;
      pinPosition = GlobalPosition;
    }

    /// <summary>
    /// Pin point to a target position.
    /// </summary>
    /// <param name="position">Target position</param>
    public void PinToPosition(Vector2 position)
    {
      pinned = true;
      pinPosition = position;
    }

    /// <summary>
    /// Unpin point from its position.
    /// </summary>
    public void Unpin()
    {
      pinned = false;
    }

    #region Lifecycle methods

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
            Update();
          }
        }

        else if (!eventScreenTouch.Pressed && eventScreenTouch.Index == touchIndex)
        {
          touchIndex = -1;
          touched = false;
          Update();
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

    #endregion

    #region Private methods

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

    private void ConstraintPinning()
    {
      if (pinned)
      {
        GlobalPosition = pinPosition;
      }
    }

    private void ConstraintPositionInSize(Vector2 worldSize)
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

    #endregion
  }
}
