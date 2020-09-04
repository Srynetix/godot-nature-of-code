using Godot;
using Utils;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Example 5.7 - Spinning Windmill.
    /// </summary>
    /// Simple body composition using torque impulse for the windmill blade.
    public class C5Example7 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 5.7:\n"
          + "Spinning Windmill\n\n"
          + "Touch screen to spawn balls";
      }

      private class WindmillBase : StaticBody2D
      {
        public Vector2 Extents = new Vector2(20, 80);

        private CollisionShape2D collisionShape2D;
        private RectangleShape2D rectangleShape2D;

        public override void _Ready()
        {
          rectangleShape2D = new RectangleShape2D();
          rectangleShape2D.Extents = Extents;
          collisionShape2D = new CollisionShape2D();
          collisionShape2D.Shape = rectangleShape2D;
          AddChild(collisionShape2D);
        }

        public override void _Draw()
        {
          DrawRect(new Rect2(-rectangleShape2D.Extents, rectangleShape2D.Extents * 2), Colors.Cornflower);
        }
      }

      private class WindmillBlade : RigidBody2D
      {
        public float Torque = 400f;
        public Vector2 Extents = new Vector2(160, 5);

        private CollisionShape2D collisionShape2D;
        private RectangleShape2D rectangleShape2D;

        public override void _Ready()
        {
          Mass = 10;
          rectangleShape2D = new RectangleShape2D();
          rectangleShape2D.Extents = Extents;
          collisionShape2D = new CollisionShape2D();
          collisionShape2D.Shape = rectangleShape2D;
          AddChild(collisionShape2D);
        }

        public override void _Draw()
        {
          DrawRect(new Rect2(-rectangleShape2D.Extents, rectangleShape2D.Extents * 2), Colors.Gray);
        }

        public override void _PhysicsProcess(float delta)
        {
          ApplyTorqueImpulse(Torque);
        }
      }

      private class Windmill : Node2D
      {
        public Vector2 BaseExtents = new Vector2(20, 80);
        public Vector2 BladeExtents = new Vector2(160, 5);
        public float BladeTorque = 6000f;

        public override void _Ready()
        {
          var windmillBase = new WindmillBase();
          windmillBase.Extents = BaseExtents;
          AddChild(windmillBase);

          var windmillBlade = new WindmillBlade();
          windmillBlade.Extents = BladeExtents;
          windmillBlade.Torque = BladeTorque;
          windmillBlade.Position = windmillBase.Position - new Vector2(0, windmillBase.Extents.y);
          AddChild(windmillBlade);

          var joint = new PinJoint2D();
          joint.NodeA = windmillBase.GetPath();
          joint.NodeB = windmillBlade.GetPath();
          joint.Softness = 0;
          windmillBlade.AddChild(joint);
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var windmill = new Windmill();
        windmill.Position = new Vector2(size.x / 2, size.y - 100);
        AddChild(windmill);

        var spawner = new SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var body = new Physics.SimpleBall();
          body.Mass = 0.25f;
          body.Radius = 10;
          body.GlobalPosition = position;
          return body;
        };
        AddChild(spawner);
      }
    }
  }
}
