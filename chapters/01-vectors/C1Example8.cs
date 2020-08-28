using Godot;
using Forces;

public class C1Example8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 1.8:\n"
      + "Velocity & constant accel.";
  }

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewportRect().Size;

    var mover = new ConstantAccelerationMover();
    mover.ConstantAcceleration = new Vector2(-0.01f, 0.01f);
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));

    AddChild(mover);
  }
}
