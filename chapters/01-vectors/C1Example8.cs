using Godot;

public class C1Example8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 1.8:\n"
      + "Velocity & constant accel.";
  }

  public class Mover : SimpleMover
  {
    protected override void UpdateAcceleration()
    {
      Acceleration = new Vector2(-0.01f, 0.01f);
    }
  }

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewportRect().Size;

    var mover = new Mover();
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
    mover.Acceleration = new Vector2(-0.01f, 0.01f);

    AddChild(mover);
  }
}
