using Godot;

public class C5Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.4:\n"
      + "Polygon Designs\n\n"
      + "Touch screen to spawn random polygons";
  }

  public class Polygon1 : Physics.SimplePolygon
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

  public class Polygon2 : Physics.SimplePolygon
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

  public class Polygon3 : Physics.SimplePolygon
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

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var wall = new Physics.SimpleWall();
    wall.MeshSize = new Vector2(size.x, 50);
    wall.Position = new Vector2(size.x / 2, size.y);
    AddChild(wall);

    var spawner = new Physics.SimpleTouchSpawner();
    spawner.Spawner = (position) =>
    {
      var chance = MathUtils.RandRangef(0, 3);
      Physics.SimplePolygon polygon = null;
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
      return polygon;
    };
    AddChild(spawner);

    int polygonCount = 10;
    for (int i = 0; i < polygonCount; ++i)
    {
      spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
    }
  }
}
