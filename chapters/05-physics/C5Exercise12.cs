using Godot;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Exercise 5.12 - Disappear on Collision.
    /// </summary>
    /// Make balls disappear on collision.
    public class C5Exercise12 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 5.12:\n"
          + "Disappear on Collision\n\n"
          + "Balls will disappear when colliding with the floor\n"
          + "Touch screen to spawn balls";
      }

      private class CollisionBall : Physics.SimpleBall
      {
        public bool Colliding = false;

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

          if (body is Physics.SimpleWall)
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
        var floor = new Physics.SimpleWall();
        floor.BodySize = new Vector2(size.x, 100);
        floor.Position = new Vector2(size.x / 2, size.y);
        AddChild(floor);

        var spawner = new Physics.SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var ball = new CollisionBall();
          ball.GlobalPosition = position;
          return ball;
        };
        AddChild(spawner);
      }
    }
  }
}
