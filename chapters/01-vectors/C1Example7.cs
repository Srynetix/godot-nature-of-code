using Godot;

public class C1Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 1.7:\n"
      + "Velocity";
  }

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewportRect().Size;

    var mover = new SimpleMover();
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
    mover.Velocity = new Vector2((float)GD.RandRange(-2.0f, 2.0f), (float)GD.RandRange(-2.0f, 2.0f));

    AddChild(mover);
  }
}
