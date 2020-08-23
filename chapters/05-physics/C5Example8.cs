using Godot;

public class C5Example8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 5.8:\n"
      + "Mouse Joint\n\n"
      + "Touch screen to move box";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var box = new SimpleBox();
    box.Position = size / 2;
    AddChild(box);
    box.AddChild(new SimpleMouseJoint());

    var floor = new SimpleWall();
    floor.BodySize = new Vector2(size.x, 100);
    floor.Position = new Vector2(size.x / 2, size.y);
    AddChild(floor);
  }
}
