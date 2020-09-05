using Godot;

namespace Examples.Chapter5
{
  /// <summary>
  /// Example 5.8 - Mouse Joint.
  /// </summary>
  /// Uses SimpleMouseJoint to simulate a mouse joint.
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

      var box = new Physics.SimpleBox();
      box.Position = size / 2;
      AddChild(box);
      box.AddChild(new Physics.SimpleMouseJoint());

      var floor = new Physics.SimpleWall();
      floor.BodySize = new Vector2(size.x, 100);
      floor.Position = new Vector2(size.x / 2, size.y);
      AddChild(floor);
    }
  }
}
