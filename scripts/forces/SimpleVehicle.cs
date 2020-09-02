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
        var targetForce = (Target.GlobalPosition - GlobalPosition).Normalized() * MaxVelocity;
        var steerForce = (targetForce - Velocity).Clamped(MaxForce);
        ApplyForce(steerForce);
      }
    }

    protected override void UpdateAcceleration()
    {
      SeekTarget();
    }
  }
}
