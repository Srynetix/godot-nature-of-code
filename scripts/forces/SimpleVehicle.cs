using Godot;
using Drawing;

namespace Forces
{
  /// <summary>
  /// Vehicle with force and steering.
  /// Tracks a target.
  /// </summary>
  public class SimpleVehicle : SimpleMover
  {
    /// <summary>Target</summary>
    public SimpleMover Target;
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
    protected virtual void SeekTarget()
    {
      if (Target != null)
      {
        var targetDiff = Target.GlobalPosition - GlobalPosition;
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
    }

    protected override void UpdateAcceleration()
    {
      SeekTarget();
    }
  }
}
