using Godot;
using Utils;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Example 5.3 - Curve Boundary.
    /// </summary>
    /// Uses SimpleStaticLines to quickly draw static bounds.
    public class C5Example3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 5.3\n"
          + "Curve Boundary\n\n"
          + "Touch screen to spawn balls";
      }

      private class CurveWall : Physics.SimpleStaticLines
      {
        public override void _Ready()
        {
          var size = GetViewportRect().Size;
          var firstHeight = 100;
          var secondHeight = 200;

          AddSegment(new Vector2(0, size.y - firstHeight), new Vector2(size.x / 2, size.y - firstHeight));
          AddSegment(new Vector2(size.x / 2, size.y - firstHeight), new Vector2(size.x, size.y - secondHeight));
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var wall = new CurveWall();
        AddChild(wall);

        var spawner = new SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var ball = new Physics.SimpleBall();
          ball.GlobalPosition = position;
          return ball;
        };
        AddChild(spawner);

        int ballCount = 10;
        for (int i = 0; i < ballCount; ++i)
        {
          spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y / 2));
        }
      }
    }
  }
}
