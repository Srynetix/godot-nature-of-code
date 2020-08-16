using Godot;

public class ConstantAccelerationMover : SimpleMover
{
  public Vector2 ConstantAcceleration = Vector2.Zero;

  protected override void UpdateAcceleration()
  {
    Acceleration = ConstantAcceleration;
  }
}
