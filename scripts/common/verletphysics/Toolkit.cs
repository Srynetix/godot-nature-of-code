using Godot;
using System.Collections.Generic;

// Inspired from:
// https://gamedevelopment.tutsplus.com/tutorials/simulate-tearable-cloth-and-ragdolls-with-simple-verlet-integration--gamedev-519

namespace VerletPhysics
{
  public class VerletEnvironment
  {
    public Vector2 Gravity = new Vector2(0, 9.8f);
  }

  public class VerletPoint : Node2D
  {
    public float Radius = 8f;
    public Color BaseColor = Colors.LightBlue;
    public float Mass = 1;
    public float Bounce = 0.9f;
    public float Friction = 0.99f;
    public float GravityScale = 1;
    public Vector2 Acceleration;
    public bool Touched = false;

    private bool pinned = false;
    private Vector2 pinPosition;
    private Vector2 prevPosition;
    private VerletWorld world;
    private List<VerletLink> links;

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
      var color = BaseColor;
      if (Touched)
      {
        color = Colors.LightGoldenrod;
      }

      DrawCircle(Vector2.Zero, Radius, color);
    }

    private void FixVelocity(Vector2 velocity)
    {
      prevPosition = GlobalPosition + velocity;
    }

    private Vector2 ComputeVelocity()
    {
      return (GlobalPosition - prevPosition);
    }

    private void ApplyGravity(VerletEnvironment environment)
    {
      ApplyForce(environment.Gravity * GravityScale);
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

    public void ApplyMovement(VerletEnvironment environment, float delta)
    {
      if (!Touched)
      {
        // Update acceleration
        ApplyGravity(environment);
      }

      var velocity = ComputeVelocity() * Friction;
      var nextPosition = GlobalPosition + velocity + Acceleration * delta;

      prevPosition = GlobalPosition;
      GlobalPosition = nextPosition;

      Acceleration = Vector2.Zero;
    }

    public void ApplyConstraints(VerletEnvironment environment, Vector2 worldSize)
    {
      if (!Touched)
      {
        foreach (VerletLink link in links)
        {
          link.Constraint(environment);
        }
      }

      ConstraintPositionInVector(environment, worldSize);
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

    private void ConstraintPositionInVector(VerletEnvironment environment, Vector2 worldSize)
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
          if (GlobalPosition.DistanceSquaredTo(eventScreenTouch.Position) < Radius * Radius)
          {
            touchIndex = eventScreenTouch.Index;
            Touched = true;
          }
        }

        else if (!eventScreenTouch.Pressed && eventScreenTouch.Index == touchIndex)
        {
          touchIndex = -1;
          Touched = false;
        }
      }

      else if (@event is InputEventScreenDrag eventScreenDrag)
      {
        if (eventScreenDrag.Index == touchIndex && Touched)
        {
          prevPosition = GlobalPosition - eventScreenDrag.Relative;
        }
      }
    }
  }

  public class VerletLink : Node2D
  {
    public float RestingDistance = 100;
    public float Stiffness = 1;
    public float TearSensitivity = 200;
    public VerletPoint A;
    public VerletPoint B;

    private VerletWorld world;

    public VerletLink(VerletWorld world, VerletPoint a, VerletPoint b)
    {
      A = a;
      B = b;
      this.world = world;
    }

    public override void _Draw()
    {
      DrawLine(A.GlobalPosition, B.GlobalPosition, Colors.Gray, 2);
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public void Constraint(VerletEnvironment environment)
    {
      if (A.Touched || B.Touched)
      {
        return;
      }

      var diff = A.GlobalPosition - B.GlobalPosition;
      var d = diff.Length();
      var difference = (RestingDistance - d) / d;

      if (d > TearSensitivity)
      {
        world.QueueLinkRemoval(this);
      }

      var imA = 1 / A.Mass;
      var imB = 1 / B.Mass;
      var scalarA = (imA / (imA + imB)) * Stiffness;
      var scalarB = Stiffness - scalarA;

      A.GlobalPosition += diff * scalarA * difference;
      B.GlobalPosition -= diff * scalarB * difference;
    }
  }

  public class VerletWorld : StaticBody2D
  {
    public VerletEnvironment Environment;
    public int ConstraintAccuracy = 1;

    private List<VerletPoint> points;
    private List<VerletLink> linksToRemove;

    public VerletWorld()
    {
      points = new List<VerletPoint>();
      linksToRemove = new List<VerletLink>();
      Environment = new VerletEnvironment();
    }

    public VerletPoint CreatePoint()
    {
      var point = new VerletPoint(this);
      points.Add(point);
      AddChild(point);

      return point;
    }

    public VerletLink CreateLink(VerletPoint A, VerletPoint B)
    {
      var link = new VerletLink(this, A, B);
      A.AddLink(link);
      AddChild(link);
      return link;
    }

    public void QueueLinkRemoval(VerletLink link)
    {
      if (!linksToRemove.Contains(link))
      {
        linksToRemove.Add(link);
      }
    }

    private void RemoveLink(VerletLink link)
    {
      foreach (VerletPoint p in points)
      {
        p.RemoveLink(link);
      }

      RemoveChild(link);
    }

    private void ProcessPoints(float delta)
    {
      var size = GetViewportRect().Size;

      for (int i = 0; i < ConstraintAccuracy; ++i)
      {
        foreach (VerletPoint point in points)
        {
          point.ApplyConstraints(Environment, size);
        }
      }

      foreach (VerletPoint point in points)
      {
        point.ApplyMovement(Environment, delta);
      }
    }

    public override void _PhysicsProcess(float delta)
    {
      ProcessPoints(delta);

      foreach (VerletLink link in linksToRemove)
      {
        RemoveLink(link);
      }
      linksToRemove.Clear();
    }
  }
}
