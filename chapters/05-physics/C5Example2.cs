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
    var leftFloor = new Physics.SimpleWall();
    leftFloor.MeshSize = new Vector2(size.x / 2.5f, floorHeight);
    leftFloor.Position = new Vector2(size.x / 2.5f / 2 + offset, size.y);
    AddChild(leftFloor);

    // Add right floor
    var rightFloor = new Physics.SimpleWall();
    rightFloor.MeshSize = new Vector2(size.x / 2.5f, floorHeight);
    rightFloor.Position = new Vector2(size.x - size.x / 2.5f / 2 - offset, size.y - offset * 2);
    AddChild(rightFloor);

    var spawner = new Physics.SimpleTouchSpawner();
    spawner.Spawner = (position) =>
    {
      var box = new Physics.SimpleBox();
      box.MeshSize = new Vector2(20, 20);
      box.GlobalPosition = position;
      return box;
    };
    AddChild(spawner);

    int boxCount = 10;
    for (int i = 0; i < boxCount; ++i)
    {
      spawner.SpawnBody(MathUtils.RandVector2(offset * 2, size.x - offset * 2, offset * 2, size.y - offset * 2));
    }
  }
}
