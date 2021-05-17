using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Example 5.4 - Polygon shapes.
    /// </summary>
    /// Uses SimplePolygon to quickly define polygons.
    public class C5Example4 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 5.4:\n"
              + "Polygon shapes\n\n"
              + "Touch screen to spawn polygons";
        }

        private class Polygon : SimplePolygon
        {
            public Polygon()
            {
                Points = new[] {
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
            const int floorHeight = 25;
            const int offset = 50;

            // Add left floor
            var leftFloor = new SimpleWall()
            {
                BodySize = new Vector2(size.x / 2.5f, floorHeight),
                Position = new Vector2((size.x / 2.5f / 2) + offset, size.y)
            };
            AddChild(leftFloor);

            // Add right floor
            var rightFloor = new SimpleWall()
            {
                BodySize = new Vector2(size.x / 2.5f, floorHeight),
                Position = new Vector2(size.x - (size.x / 2.5f / 2) - offset, size.y - (offset * 2))
            };
            AddChild(rightFloor);

            var spawner = new SimpleTouchSpawner()
            {
                SpawnFunction = (position) =>
                {
                    return new Polygon()
                    {
                        GlobalPosition = position
                    };
                }
            };
            AddChild(spawner);

            const int polygonCount = 10;
            for (int i = 0; i < polygonCount; ++i)
            {
                spawner.SpawnBody(MathUtils.RandVector2(offset * 2, size.x - (offset * 2), offset * 2, size.y - (offset * 2)));
            }
        }
    }
}
