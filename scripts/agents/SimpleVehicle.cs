using Godot;
using Drawing;
using Forces;

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
      var predictFactor = 25;
      var dirFactor = 10;

      var predict = Velocity.Normalized() * predictFactor;
      var predictPos = GlobalPosition + predict;
      var a = TargetPath.Start;
      var b = TargetPath.End;
      var normalPoint = predictPos.GetNormalPoint(a, b);
      var dir = (a - b).Normalized() * dirFactor;
      var target = normalPoint + dir;

      var dist = normalPoint.DistanceTo(predictPos);
      if (dist > TargetPath.Radius)
      {
        Seek(target);
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
