using Godot;

public class C5Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 5.4:\n"
      + "Polygon shapes\n\n"
      + "Touch screen to spawn polygons";
  }

  public class Polygon : Physics.SimplePolygon
  {
    public Polygon()
    {
      Points = new Vector2[] {
        new Vector2(-15, 25),
        new Vector2(15, 0),
        new Vector2(20, -15),
        new Vector2(-10, -10),
      };

      BaseColor = Colors.White;
    }
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
      var polygon = new Polygon();
      polygon.GlobalPosition = position;
      return polygon;
    };
    AddChild(spawner);

    int polygonCount = 10;
    for (int i = 0; i < polygonCount; ++i)
    {
      spawner.SpawnBody(MathUtils.RandVector2(offset * 2, size.x - offset * 2, offset * 2, size.y - offset * 2));
    }
  }
}
