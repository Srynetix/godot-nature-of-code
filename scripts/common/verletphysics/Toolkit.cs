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
    private const int mouseDetectionRadiusThreshold = 10;

    public float Radius = 8f;
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
      if (!touched)
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
      foreach (VerletLink link in links)
      {
        link.Constraint(environment);
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

  public class VerletLink : Node2D
  {
    public float RestingDistance = 100;
    public float Stiffness = 1;
    public float TearSensitivity = 200;
    public VerletPoint A;
    public VerletPoint B;
    public bool Drawing = true;

    private VerletWorld world;

    public VerletLink(VerletWorld world, VerletPoint a, VerletPoint b)
    {
      A = a;
      B = b;
      this.world = world;
    }

    public override void _Draw()
    {
      if (Drawing)
      {
        DrawLine(A.GlobalPosition, B.GlobalPosition, Colors.Gray, 2);
      }
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public void Constraint(VerletEnvironment environment)
    {
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

    public VerletLink[] CreateChain(Vector2[] points, float restingDistance = 30, float tearSensitivity = 50, float stiffness = 1, bool drawIntermediatePoints = true)
    {
      if (points.Length < 2)
      {
        GD.PrintErr("Bad points length for chain. Need to be >= 2");
        return null;
      }

      // First point is static
      int linkCount = points.Length - 1;
      var links = new VerletLink[linkCount];
      VerletPoint prevPoint = CreatePoint();
      prevPoint.MoveToPosition(points[0]);
      prevPoint.PinToCurrentPosition();

      for (int i = 1; i < points.Length; ++i)
      {
        var currPoint = CreatePoint();
        currPoint.MoveToPosition(points[i]);

        if (i != 1 && !drawIntermediatePoints)
        {
          prevPoint.Drawing = false;
        }

        var link = CreateLink(prevPoint, currPoint);
        link.RestingDistance = restingDistance;
        link.TearSensitivity = tearSensitivity;
        link.Stiffness = stiffness;
        links[i - 1] = link;

        prevPoint = currPoint;
      }

      return links;
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
