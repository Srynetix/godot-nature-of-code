using Godot;
using Physics;
using Utils;

namespace Examples
{
    /// <summary>
    /// Chapter 5 - Physics Libraries.
    /// </summary>
    namespace Chapter5
    {
        /// <summary>
        /// Example 5.2 - Boxes and Walls.
        /// </summary>
        /// Uses RigidBody2D / StaticBody2D prefabs (SimpleWall / SimpleBox) to quickly build scenes with physics.
        /// Also uses SimpleTouchSpawner to spawn elements on touch.
        public class C5Example2 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 5.2:\n"
                  + "Boxes and Walls\n\n"
                  + "Touch screen to spawn boxes";
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
                        return new SimpleBox()
                        {
                            BodySize = new Vector2(20, 20),
                            GlobalPosition = position
                        };
                    }
                };
                AddChild(spawner);

                const int boxCount = 10;
                for (int i = 0; i < boxCount; ++i)
                {
                    spawner.SpawnBody(MathUtils.RandVector2(offset * 2, size.x - (offset * 2), offset * 2, size.y - (offset * 2)));
                }
            }
        }
    }
}
