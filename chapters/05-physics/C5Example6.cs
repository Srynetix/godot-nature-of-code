using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Example 5.6 - Distance Joint.
    /// </summary>
    /// Simulate distance joint using PinJoint2D.
    public class C5Example6 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 5.6:\n"
              + "Distance Joint\n\n"
              + "Using PinJoint2D to simulate distance joints.";
        }

        private class DoubleBall : Node2D
        {
            public float OutlineWidth = 2;
            public Color OutlineColor = Colors.LightBlue;
            public Color BaseColor = Colors.White;
            public float Distance = 10;
            public float Radius = 20;

            private SimpleBall ball1;
            private SimpleBall ball2;

            public override void _Ready()
            {
                ball1 = new SimpleBall
                {
                    Radius = Radius,
                    Position = new Vector2(-Distance, -Distance)
                };
                ball2 = new SimpleBall
                {
                    Radius = Radius,
                    Position = new Vector2(Distance, Distance)
                };
                AddChild(ball1);
                AddChild(ball2);

                var join = new PinJoint2D
                {
                    NodeA = ball1.GetPath(),
                    NodeB = ball2.GetPath(),
                    Softness = 0.1f
                };
                ball1.AddChild(join);
            }

            public override void _Draw()
            {
                DrawLine(ball1.Position, ball2.Position, Colors.White, 2);
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
                    return new DoubleBall
                    {
                        Distance = 20,
                        Radius = 10,
                        RotationDegrees = MathUtils.RandRangef(0, 360),
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
