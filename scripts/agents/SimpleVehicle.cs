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
    /// <summary>Debug draw path</summary>
    public bool DebugDrawPath = false;
    /// <summary>Enable separation group behavior</summary>
    public bool SeparationEnabled = false;
    /// <summary>Enable cohesion group behavior</summary>
    public bool CohesionEnabled = false;
    /// <summary>Vehicle group list</summary>
    public List<SimpleVehicle> VehicleGroupList = null;

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
    }

    /// <summary>
    /// Separate from other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    protected void Separate(List<SimpleVehicle> vehicles)
    {
      float desiredSeparation = Radius * 4;
      var sum = Vector2.Zero;
      int count = 0;

      foreach (var vehicle in vehicles)
      {
        float d = GlobalPosition.DistanceTo(vehicle.GlobalPosition);
        if (d > 0 && d < desiredSeparation)
        {
          sum += (GlobalPosition - vehicle.GlobalPosition).Normalized() / d;
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * MaxVelocity;
        var steer = (sum - Velocity).Clamped(MaxForce);
        ApplyForce(steer);
      }
    }

    /// <summary>
    /// Regroup with other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    protected void Regroup(List<SimpleVehicle> vehicles)
    {
      float separationLimit = Radius * 2;
      var sum = Vector2.Zero;
      int count = 0;

      foreach (var vehicle in vehicles)
      {
        float d = GlobalPosition.DistanceTo(vehicle.GlobalPosition);
        if (d > 0 && d > separationLimit)
        {
          sum += (GlobalPosition - vehicle.GlobalPosition).Normalized() / d;
          count++;
        }
      }

      if (count > 0)
      {
        sum = (sum / count).Normalized() * -MaxVelocity;
        var steer = (sum - Velocity).Clamped(MaxForce);
        ApplyForce(steer);
      }
    }

    /// <summary>
    /// Drive and steer towards target.
    /// </summary>
    protected void SeekTarget()
    {
      Seek(Target.GlobalPosition);
    }

    /// <summary>
    /// Drive and steer towards target position.
    /// </summary>
    /// <param name="position">Target position</param>
    protected virtual void Seek(Vector2 position)
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

      var steerForce = (targetDiff - Velocity).Clamped(MaxForce);
      ApplyForce(steerForce);
    }

    /// <summary>
    /// Drive and steer following a flow field.
    /// </summary>
    protected void FollowFlow()
    {
      var tgtDirection = TargetFlow.Lookup(GlobalPosition);
      if (tgtDirection == Vector2.Zero)
      {
        // Ignore empty lookup
        return;
      }

      var target = tgtDirection * MaxVelocity;
      var steer = (target - Velocity).Clamped(MaxForce);
      ApplyForce(steer);
    }

    /// <summary>
    /// Drive and steer following a path.
    /// Always follow the path from A to B.
    /// </summary>
    protected void FollowPath()
    {
      var minDist = float.MaxValue;

      var predict = Velocity.Normalized() * DetectionScanLength;
      var predictPos = GlobalPosition + predict;

      Vector2? target = null;
      Vector2? targetNormalPoint = null;
      var pointCount = TargetPath.Points.Count;

      for (int i = 0; i < TargetPath.Points.Count; ++i)
      {
        // Ignore last iteration if not looping
        if (!TargetPath.Looping && i == TargetPath.Points.Count - 1)
        {
          break;
        }

        var a = TargetPath.Points[i];
        var b = TargetPath.Points[(i + 1) % TargetPath.Points.Count];
        var normalPoint = predictPos.GetNormalPoint(a, b);
        var dir = b - a;

        // Out of bounds
        if (normalPoint.x < Mathf.Min(a.x, b.x) || normalPoint.x > Mathf.Max(a.x, b.x)
          || normalPoint.y < Mathf.Min(a.y, b.y) || normalPoint.y > Mathf.Max(a.y, b.y))
        {
          normalPoint = b;

          if (TargetPath.Looping || i != TargetPath.Points.Count - 2)
          {
            // Get two next points
            var a2 = b;
            var b2 = TargetPath.Points[(i + 2) % TargetPath.Points.Count];
            dir = b2 - a2;
          }
        }

        var dist = normalPoint.DistanceTo(predictPos);
        if (dist < minDist)
        {
          minDist = dist;
          targetNormalPoint = normalPoint;
          target = targetNormalPoint + dir.Normalized() * DetectionTargetOffset;
        }
      }

      // Set debug values
      debugNormalPoint = targetNormalPoint;
      debugPredictPos = predictPos;
      debugTargetPoint = target;

      if (target.HasValue)
      {
        Seek(target.Value);
      }
    }

    protected override void UpdateAcceleration()
    {
      if (Target != null)
      {
        SeekTarget();
      }

      if (TargetFlow != null)
      {
        FollowFlow();
      }

      if (TargetPath != null)
      {
        FollowPath();
      }

      if (SeparationEnabled && VehicleGroupList.Count > 0)
      {
        Separate(VehicleGroupList);
      }

      else if (CohesionEnabled && VehicleGroupList.Count > 0)
      {
        Regroup(VehicleGroupList);
      }
    }

    public override void _Draw()
    {
      base._Draw();

      if (DebugDrawPath && TargetPath != null)
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
    }
  }
}
