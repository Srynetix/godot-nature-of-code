using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.12 - Disappear on Collision.
    /// </summary>
    /// Make balls disappear on collision.
    public class C5Exercise12 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.12:\n"
              + "Disappear on Collision\n\n"
              + "Balls will disappear when colliding with the floor\n"
              + "Touch screen to spawn balls";
        }

        private class CollisionBall : SimpleBall
        {
            public bool Colliding;

            public CollisionBall()
            {
                ContactMonitor = true;
                ContactsReported = 1;
            }

            public override void _Ready()
            {
                base._Ready();

                Connect("body_entered", this, nameof(OnBodyEntered));
                Connect("body_exited", this, nameof(OnBodyExited));
            }

            public void OnBodyEntered(PhysicsBody2D body)
            {
                Colliding = true;

                if (body is SimpleWall)
                {
                    // Remove on wall
                    QueueFree();
                }
            }

            public void OnBodyExited(PhysicsBody2D body)
            {
                Colliding = false;
            }

            public override void _Process(float delta)
            {
                if (Colliding)
                {
                    BaseColor = Colors.Red;
                }
                else
                {
                    BaseColor = Colors.LightBlue;
                }
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var floor = new SimpleWall()
            {
                BodySize = new Vector2(size.x, 100),
                Position = new Vector2(size.x / 2, size.y)
            };
            AddChild(floor);

            var spawner = new SimpleTouchSpawner()
            {
                SpawnFunction = (position) =>
                {
                    return new CollisionBall()
                    {
                        GlobalPosition = position
                    };
                }
            };
            AddChild(spawner);
        }
    }
}
