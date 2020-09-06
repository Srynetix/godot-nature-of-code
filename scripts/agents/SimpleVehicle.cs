using Godot;
using Drawing;
using Forces;
using System.Collections.Generic;

/// <summary>
/// Autonomous agents related primitives.
/// </summary>
namespace Agents
{
  /// <summary>
  /// Vehicle with force and steering.
  /// Tracks a target.
  /// </summary>
  public class SimpleVehicle : SimpleMover
  {
    /// <summary>Target</summary>
    public SimpleMover Target;

    /// <summary>Flow target</summary>
    public SimpleFlowField TargetFlow;

    /// <summary>Path target</summary>
    public SimplePath TargetPath;

    /// <summary>Max force</summary>
    public float MaxForce = 0.1f;

    /// <summary>Arrive distance. Use `-1` to disable.</summary>
    public float ArriveDistance = -1;

    /// <summary>Detection scan length</summary>
    public float DetectionScanLength = 25;

    /// <summary>Detection target offset</summary>
    public float DetectionTargetOffset = 25;

    /// <summary>Detection alignment radius</summary>
    public float DetectionAlignmentRadius = 50;

    /// <summary>Debug draw</summary>
    public bool DebugDraw = false;

    /// <summary>Enable separation group behavior</summary>
    public bool SeparationEnabled = false;

    /// <summary>Enable cohesion group behavior</summary>
    public bool CohesionEnabled = false;

    /// <summary>Enable alignment group behavior</summary>
    public bool AlignmentEnabled = false;

    /// <summary>Enable lateral move group behavior</summary>
    public bool LateralMoveEnabled = false;

    /// <summary>Vehicle group list</summary>
    public List<SimpleVehicle> VehicleGroupList = null;

    /// <summary>Seek force factor</summary>
    public float SeekForceFactor = 1;

    /// <summary>Separation force factor</summary>
    public float SeparationForceFactor = 1;

    /// <summary>Cohesion force factor</summary>
    public float CohesionForceFactor = 1;

    /// <summary>Alignment force factor</summary>
    public float AlignmentForceFactor = 1;

    /// <summary>Lateral move force factor</summary>
    public float LateralMoveForceFactor = 1;

    private Vector2? debugPredictPos = null;
    private Vector2? debugNormalPoint = null;
    private Vector2? debugTargetPoint = null;

    /// <summary>
    /// Create a default vehicle.
    /// </summary>
    public SimpleVehicle()
    {
      Mesh.MeshType = SimpleMesh.TypeEnum.Square;
      Mesh.MeshSize = new Vector2(40, 20);

      SyncRotationOnVelocity = true;
      MaxVelocity = 4;
      MaxForce = 0.4f;

      Name = "SimpleVehicle";

      // Disable physics logic
      collisionShape2D.Disabled = true;
    }

    /// <summary>
    /// Separate from other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    /// <returns>Steer force</returns>
    protected Vector2 Separate(List<SimpleVehicle> vehicles)
    {
      float desiredSeparation = Radius * 4;
      var sum = Vector2.Zero;
      int count = 0;

      foreach (var vehicle in vehicles)
      {
        float d = GlobalPosition.DistanceSquaredTo(vehicle.GlobalPosition);
        if (d > 0 && d < desiredSeparation * desiredSeparation)
        {
          sum += (GlobalPosition - vehicle.GlobalPosition).Normalized() / Mathf.Sqrt(d);
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * MaxVelocity;
        return (sum - Velocity).Clamped(MaxForce);
      }
      else
      {
        return Vector2.Zero;
      }
    }

    /// <summary>
    /// Regroup with other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    /// <returns>Steer force</returns>
    protected Vector2 Regroup(List<SimpleVehicle> vehicles)
    {
      float separationLimit = Radius * 2;
      var sum = Vector2.Zero;
      int count = 0;

      foreach (var vehicle in vehicles)
      {
        float d = GlobalPosition.DistanceSquaredTo(vehicle.GlobalPosition);
        if (d > 0 && d > separationLimit * separationLimit)
        {
          sum += (GlobalPosition - vehicle.GlobalPosition).Normalized() / Mathf.Sqrt(d);
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * -MaxVelocity;
        return (sum - Velocity).Clamped(MaxForce);
      }
      else
      {
        return Vector2.Zero;
      }
    }

    /// <summary>
    /// Drive and steer towards target position.
    /// </summary>
    /// <param name="position">Target position</param>
    /// <returns>Steer force</returns>
    protected virtual Vector2 Seek(Vector2 position)
    {
      var targetDiff = position - GlobalPosition;
      var targetDist = targetDiff.Length();
      targetDiff = targetDiff.Normalized();

      if (ArriveDistance > 0 && targetDist < ArriveDistance)
      {
        targetDiff *= MathUtils.Map(targetDist, 0, ArriveDistance, 0, MaxVelocity);
      }
      else
      {
        targetDiff *= MaxVelocity;
      }

      return (targetDiff - Velocity).Clamped(MaxForce);
    }

    /// <summary>
    /// Drive and steer following a flow field.
    /// </summary>
    /// <returns>Steer force</returns>
    protected Vector2 FollowFlow()
    {
      var tgtDirection = TargetFlow.Lookup(GlobalPosition);
      if (tgtDirection == Vector2.Zero)
      {
        // Ignore empty lookup
        return Vector2.Zero;
      }

      var target = tgtDirection * MaxVelocity;
      return (target - Velocity).Clamped(MaxForce);
    }

    /// <summary>
    /// Drive and steer following a path.
    /// Always follow the path from A to B.
    /// </summary>
    /// <returns>Steer force</returns>
    protected Vector2 FollowPath()
    {
      var minDist = float.MaxValue;

      var predict = Velocity.Normalized() * DetectionScanLength;
      var predictPos = GlobalPosition + predict;

      Vector2? target = null;
      Vector2? targetNormalPoint = null;
      var pointCount = TargetPath.Points.Count;

      for (int i = 0; i < pointCount; ++i)
      {
        // Ignore last iteration if not looping
        if (!TargetPath.Looping && i == pointCount - 1)
        {
          break;
        }

        var a = TargetPath.Points[i];
        var b = TargetPath.Points[(i + 1) % pointCount];
        var normalPoint = predictPos.GetNormalPoint(a, b);
        var dir = b - a;

        // Out of bounds
        if (normalPoint.x < Mathf.Min(a.x, b.x) || normalPoint.x > Mathf.Max(a.x, b.x)
          || normalPoint.y < Mathf.Min(a.y, b.y) || normalPoint.y > Mathf.Max(a.y, b.y))
        {
          normalPoint = b;

          if (TargetPath.Looping || i != pointCount - 2)
          {
            // Get two next points
            var a2 = b;
            var b2 = TargetPath.Points[(i + 2) % pointCount];
            dir = b2 - a2;
          }
        }

        var dist = normalPoint.DistanceTo(predictPos);
        if (dist < minDist)
        {
          minDist = dist;
          targetNormalPoint = normalPoint;
          target = targetNormalPoint + (dir.Normalized() * DetectionTargetOffset);
        }
      }

      // Set debug values
      debugNormalPoint = targetNormalPoint;
      debugPredictPos = predictPos;
      debugTargetPoint = target;

      if (target.HasValue)
      {
        return Seek(target.Value);
      }
      else
      {
        return Vector2.Zero;
      }
    }

    /// <summary>
    /// Align with other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    /// <returns>Steer force</returns>
    protected virtual Vector2 Align(List<SimpleVehicle> vehicles)
    {
      var sum = Vector2.Zero;
      int count = 0;
      foreach (var vehicle in vehicles)
      {
        var d = GlobalPosition.DistanceSquaredTo(vehicle.GlobalPosition);
        if (d > 0 && d < DetectionAlignmentRadius * DetectionAlignmentRadius)
        {
          sum += vehicle.Velocity;
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * MaxVelocity;
        return (sum - Velocity).Clamped(MaxForce);
      }
      else
      {
        return Vector2.Zero;
      }
    }

    /// <summary>
    /// Move laterally from other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    /// <returns>Steer force</returns>
    protected virtual Vector2 MoveLaterally(List<SimpleVehicle> vehicles)
    {
      var sum = Vector2.Zero;
      int count = 0;

      // Use an arbitrary target position on the radius circle
      var detectedPos = GlobalPosition + new Vector2(DetectionAlignmentRadius, 0).Rotated(GlobalRotation);

      foreach (var vehicle in vehicles)
      {
        var dp = detectedPos.DistanceSquaredTo(vehicle.GlobalPosition);

        // Compare the distance between the target position and the neighbor
        if (dp > 0 && dp < (DetectionAlignmentRadius * DetectionAlignmentRadius) / 2)
        {
          var diff = vehicle.GlobalPosition - GlobalPosition;
          sum += diff.Rotated(Mathf.Pi / 2);
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * MaxVelocity;
        return (sum - Velocity).Clamped(MaxForce);
      }
      else
      {
        return Vector2.Zero;
      }
    }

    protected override void UpdateAcceleration()
    {
      var forces = Vector2.Zero;

      if (Target != null)
      {
        forces += Seek(Target.GlobalPosition) * SeekForceFactor;
      }

      if (TargetFlow != null)
      {
        forces += FollowFlow() * SeekForceFactor;
      }

      if (TargetPath != null)
      {
        forces += FollowPath() * SeekForceFactor;
      }

      if (VehicleGroupList?.Count > 0)
      {
        if (SeparationEnabled)
        {
          forces += Separate(VehicleGroupList) * SeparationForceFactor;
        }

        if (AlignmentEnabled)
        {
          forces += Align(VehicleGroupList) * AlignmentForceFactor;
        }

        if (CohesionEnabled)
        {
          forces += Regroup(VehicleGroupList) * CohesionForceFactor;
        }

        if (LateralMoveEnabled)
        {
          forces += MoveLaterally(VehicleGroupList) * LateralMoveForceFactor;
        }
      }

      ApplyForce(forces);
    }

    public override void _Draw()
    {
      if (DebugDraw && TargetPath != null)
      {
        if (debugNormalPoint.HasValue && debugPredictPos.HasValue && debugTargetPoint.HasValue)
        {
          var predictPos = (debugPredictPos.Value - GlobalPosition).Rotated(-GlobalRotation);
          var normalPos = (debugNormalPoint.Value - GlobalPosition).Rotated(-GlobalRotation);
          var targetPos = (debugTargetPoint.Value - GlobalPosition).Rotated(-GlobalRotation);

          DrawLine(Vector2.Zero, predictPos, Colors.OrangeRed);
          DrawLine(predictPos, normalPos, Colors.BlueViolet);
          DrawLine(normalPos, targetPos, Colors.Orange);
          DrawCircle(targetPos, 5, Colors.OrangeRed);
        }
      }

      if (DebugDraw && (AlignmentEnabled || LateralMoveEnabled))
      {
        DrawCircle(Vector2.Zero, DetectionAlignmentRadius, Colors.Azure.WithAlpha(64));
      }
    }
  }
}
