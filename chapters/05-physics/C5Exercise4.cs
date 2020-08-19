using Godot;

public class C5Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.4:\n"
      + "Polygon Designs\n\n"
      + "Touch screen to spawn random polygons";
  }

  public class Polygon1 : SimplePolygon
  {
    public Polygon1()
    {
      Points = new Vector2[] {
        new Vector2(-10, 10),
        new Vector2(10, 10),
        new Vector2(5, 5),
        new Vector2(-10, 5)
      };

      BaseColor = Colors.Aqua;
    }
  }

  public class Polygon2 : SimplePolygon
  {
    public Polygon2()
    {
      Points = new Vector2[] {
        new Vector2(-10, 10),
        new Vector2(-10, 10),
        new Vector2(5, -5),
        new Vector2(-10, 5)
      };

      BaseColor = Colors.BlueViolet;
    }
  }

  public class Polygon3 : SimplePolygon
  {
    public Polygon3()
    {
      Points = new Vector2[] {
        new Vector2(-10, 10),
        new Vector2(10, 10),
        new Vector2(5, -5),
        new Vector2(10, 5)
      };

      BaseColor = Colors.IndianRed;
    }
  }

  private void SpawnRandomPolygon(Vector2 position)
  {
    var chance = Utils.RandRangef(0, 3);
    SimplePolygon polygon = null;
    if (chance < 1)
    {
      polygon = new Polygon1();
    }
    else if (chance < 2)
    {
      polygon = new Polygon2();
    }
    else
    {
      polygon = new Polygon3();
    }

    polygon.GlobalPosition = position;
    AddChild(polygon);
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var wall = new SimpleWall();
    wall.BodySize = new Vector2(size.x, 50);
    wall.Position = new Vector2(size.x / 2, size.y);
    AddChild(wall);

    int polygonCount = 10;
    for (int i = 0; i < polygonCount; ++i)
    {
      SpawnRandomPolygon(Utils.RandVector2(0, size.x, 0, size.y));
    }
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        SpawnRandomPolygon(eventScreenTouch.Position);
      }
    }

    if (@event is InputEventScreenDrag eventScreenDrag)
    {
      SpawnRandomPolygon(eventScreenDrag.Position);
    }
  }
}
