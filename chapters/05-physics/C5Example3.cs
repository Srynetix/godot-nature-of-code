using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Example 5.3 - Curve Boundary.
  /// </summary>
  /// Uses SimpleStaticLines to quickly draw static bounds.
  public class C5Example3 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 5.3\n"
        + "Curve Boundary\n\n"
        + "Touch screen to spawn balls";
    }

    private class CurveWall : SimpleStaticLines
    {
      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        const int firstHeight = 100;
        const int secondHeight = 200;

        AddSegment(new Vector2(0, size.y - firstHeight), new Vector2(size.x / 2, size.y - firstHeight));
        AddSegment(new Vector2(size.x / 2, size.y - firstHeight), new Vector2(size.x, size.y - secondHeight));
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var wall = new CurveWall();
      AddChild(wall);

      var spawner = new SimpleTouchSpawner {
        SpawnFunction = (position) =>
        {
          return new SimpleBall {
            GlobalPosition = position
          };
        }
      };
      AddChild(spawner);

      const int ballCount = 10;
      for (int i = 0; i < ballCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y / 2));
      }
    }
  }
}
