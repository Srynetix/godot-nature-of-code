using Godot;

public class C1Example9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 1.9:\n"
      + "Velocity & random accel.";
  }

  public class Mover : SimpleMover
  {
    protected override void UpdateAcceleration()
    {
      Acceleration = new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1));
    }
  }

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewportRect().Size;

    var mover = new Mover();
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
    AddChild(mover);
  }
}
