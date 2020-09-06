using Godot;
using Forces;

namespace Examples.Chapter1
{
  /// <summary>
  /// Example 1.8 - Velocity and constant acceleration.
  /// </summary>
  /// Uses a ConstantAccelerationMover with a fixed constant acceleration.
  public class C1Example8 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 1.8:\n"
        + "Velocity & constant accel.";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var mover = new ConstantAccelerationMover
      {
        ConstantAcceleration = new Vector2(-0.01f, 0.01f),
        Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y))
      };

      AddChild(mover);
    }
  }
}
