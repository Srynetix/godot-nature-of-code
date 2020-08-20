using Godot;

public class C5Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.2:\n"
      + "Boxes and Walls\n\n"
      + "Touch screen to spawn boxes";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var floorHeight = 25;
    var offset = 50;

    // Add left floor
    var leftFloor = new SimpleWall();
    leftFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    leftFloor.Position = new Vector2(size.x / 2.5f / 2 + offset, size.y);
    AddChild(leftFloor);

    // Add right floor
    var rightFloor = new SimpleWall();
    rightFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    rightFloor.Position = new Vector2(size.x - size.x / 2.5f / 2 - offset, size.y - offset * 2);
    AddChild(rightFloor);

    int boxCount = 10;
    for (int i = 0; i < boxCount; ++i)
    {
      SpawnBox(Utils.RandVector2(offset * 2, size.x - offset * 2, offset * 2, size.y - offset * 2));
    }
  }

  private void SpawnBox(Vector2 position)
  {
    var box = new SimpleBox();
    box.BodySize = new Vector2(20, 20);
    box.GlobalPosition = position;
    AddChild(box);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        SpawnBox(eventScreenTouch.Position);
      }
    }

    if (@event is InputEventScreenDrag eventScreenDrag)
    {
      SpawnBox(eventScreenDrag.Position);
    }
  }
}
