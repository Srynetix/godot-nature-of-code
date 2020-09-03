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

    /// <summary>
    /// Create a default vehicle.
    /// </summary>
    public SimpleVehicle()
    {
      Mesh.MeshType = SimpleMesh.TypeEnum.Square;
      Mesh.MeshSize = new Vector2(40, 20);

      SyncRotationOnVelocity = true;
      MaxVelocity = 4;
    }

    /// <summary>
    /// Separate from other vehicles.
    /// </summary>
    /// <param name="vehicles">Other vehicles</param>
    public void Separate(List<SimpleVehicle> vehicles)
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
    public void Regroup(List<SimpleVehicle> vehicles)
    {
      float separationLimit = Radius * 4;
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
    /// </summary>
    protected void FollowPath()
    {
      var predictFactor = 50;
      var dirFactor = 25;
      var minDist = float.MaxValue;

      var predict = Velocity.Normalized() * predictFactor;
      var predictPos = GlobalPosition + predict;
      Vector2? target = null;

      for (int i = 0; i < TargetPath.Points.Count - 1; ++i)
      {
        var a = TargetPath.Points[i];
        var b = TargetPath.Points[i + 1];
        var normalPoint = predictPos.GetNormalPoint(a, b);
        if (normalPoint.x < Mathf.Min(a.x, b.x) || normalPoint.x > Mathf.Max(b.x, a.x))
        {
          normalPoint = b;
        }

        var dir = (b - a).Normalized() * dirFactor;
        var dist = normalPoint.DistanceTo(predictPos);
        if (dist < minDist)
        {
          minDist = dist;
          target = normalPoint;
        }
      }

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
    }
  }
}
