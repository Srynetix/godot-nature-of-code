using Godot;

public class C5Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 5.4:\n"
      + "Polygon shapes\n\n"
      + "Touch screen to spawn polygons";
  }

  public class Polygon : SimplePolygon
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
    var leftFloor = new SimpleWall();
    leftFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    leftFloor.Position = new Vector2(size.x / 2.5f / 2 + offset, size.y);
    AddChild(leftFloor);

    // Add right floor
    var rightFloor = new SimpleWall();
    rightFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    rightFloor.Position = new Vector2(size.x - size.x / 2.5f / 2 - offset, size.y - offset * 2);
    AddChild(rightFloor);

    int polygonCount = 10;
    for (int i = 0; i < polygonCount; ++i)
    {
      SpawnPolygon(Utils.RandVector2(offset * 2, size.x - offset * 2, offset * 2, size.y - offset * 2));
    }
  }

  private void SpawnPolygon(Vector2 position)
  {
    var polygon = new Polygon();
    polygon.GlobalPosition = position;
    AddChild(polygon);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        SpawnPolygon(eventScreenTouch.Position);
      }
    }

    if (@event is InputEventScreenDrag eventScreenDrag)
    {
      SpawnPolygon(eventScreenDrag.Position);
    }
  }
}
