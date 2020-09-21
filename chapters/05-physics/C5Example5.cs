using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Example 5.5 - Multiple Shapes / One Body.
    /// </summary>
    /// Combine shapes using CollisionShape2D.
    public class C5Example5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 5.5:\n"
              + "Multiple Shapes / One Body\n\n"
              + "Touch screen to spawn bodies";
        }

        private class MultiShapeBody : RigidBody2D
        {
            private CollisionShape2D bodyCollisionShape;
            private RectangleShape2D bodyShape;
            private CollisionShape2D headCollisionShape;
            private CircleShape2D headShape;

            public override void _Ready()
            {
                bodyCollisionShape = new CollisionShape2D();
                bodyShape = new RectangleShape2D { Extents = new Vector2(10, 20) };
                bodyCollisionShape.Shape = bodyShape;
                AddChild(bodyCollisionShape);

                headCollisionShape = new CollisionShape2D();
                headShape = new CircleShape2D { Radius = 15 };
                headCollisionShape.Position = new Vector2(0, -30);
                headCollisionShape.Shape = headShape;
                AddChild(headCollisionShape);
            }

            public override void _Draw()
            {
                DrawRect(new Rect2(-bodyShape.Extents, bodyShape.Extents * 2), Colors.White);
                DrawCircle(headCollisionShape.Position, headShape.Radius, Colors.LightBlue);
            }

            public override void _Process(float delta)
            {
                Update();
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            const int floorHeight = 25;
            const int offset = 50;

            // Add left floor
            var leftFloor = new SimpleWall
            {
                BodySize = new Vector2(size.x / 2.5f, floorHeight),
                Position = new Vector2((size.x / 2.5f / 2) + offset, size.y)
            };
            AddChild(leftFloor);

            // Add right floor
            var rightFloor = new SimpleWall
            {
                BodySize = new Vector2(size.x / 2.5f, floorHeight),
                Position = new Vector2(size.x - (size.x / 2.5f / 2) - offset, size.y - (offset * 2))
            };
            AddChild(rightFloor);

            var spawner = new SimpleTouchSpawner
            {
                SpawnFunction = (position) =>
                {
                    return new MultiShapeBody
                    {
                        GlobalPosition = position
                    };
                }
            };
            AddChild(spawner);

            const int bodyCount = 10;
            for (int i = 0; i < bodyCount; ++i)
            {
                spawner.SpawnBody(MathUtils.RandVector2(offset * 2, size.x - (offset * 2), offset * 2, size.y - (offset * 2)));
            }
        }
    }
}
