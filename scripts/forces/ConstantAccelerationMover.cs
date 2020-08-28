using Godot;

namespace Forces
{
  /// <summary>
  /// Simple mover with constant acceleration.
  /// </summary>
  public class ConstantAccelerationMover : SimpleMover
  {
    /// <summary>Constant acceleration value</summary>
    public Vector2 ConstantAcceleration = Vector2.Zero;

    protected override void UpdateAcceleration()
    {
      Acceleration = ConstantAcceleration;
    }
  }
}
