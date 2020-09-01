using Godot;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Exercise 5.10 - Attractor With Physics.
    /// </summary>
    /// Attraction principle using physics forces.
    public class C5Exercise10 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 5.10:\n"
          + "Attractor With Physics\n\n"
          + "Touch screen to spawn balls";
      }

      private class PhysicsAttractor : StaticBody2D
      {
        public float Radius = 20.0f;
        public float Mass = 1000f;
        public float Gravitation = 1.0f;
        public float MinForce = 25;
        public float MaxForce = 100;
        public bool Drawing = true;
        public Color BaseColor = Colors.LightGoldenrod;

        public override void _Ready()
        {
          var collisionShape = new CollisionShape2D();
          var shape = new CircleShape2D();
          shape.Radius = Radius;
          collisionShape.Shape = shape;
          AddChild(collisionShape);
        }

        public virtual Vector2 Attract(RigidBody2D mover)
        {
          var force = GlobalPosition - mover.GlobalPosition;
          var length = Mathf.Clamp(force.Length(), MinForce, MaxForce);
          float strength = (Gravitation * Mass * mover.Mass) / (length * length);
          return force.Normalized() * strength;
        }

        public override void _Draw()
        {
          if (Drawing)
          {
            DrawCircle(Vector2.Zero, Radius, BaseColor);
          }
        }

        protected void AttractNodes()
        {
          // For each mover
          foreach (var n in GetTree().GetNodesInGroup("rigidbody"))
          {
            var body = (RigidBody2D)n;
            var force = Attract(body);
            body.ApplyImpulse(Vector2.Zero, force);
          }
        }

        public override void _PhysicsProcess(float delta)
        {
          AttractNodes();
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var attractor = new PhysicsAttractor();
        attractor.Position = size / 2;
        AddChild(attractor);

        var spawner = new Physics.SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var body = new Physics.SimpleBall();
          body.AddToGroup("rigidbody");
          body.GravityScale = 0;
          body.Mass = 2;
          body.GlobalPosition = position;
          return body;
        };
        AddChild(spawner);

        // Two initial balls
        spawner.SpawnBody(new Vector2(size.x * 0.25f, size.y * 0.25f));
        spawner.SpawnBody(new Vector2(size.x * 0.75f, size.y * 0.75f));
      }
    }
  }
}
