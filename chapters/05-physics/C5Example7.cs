using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Example 5.7 - Spinning Windmill.
  /// </summary>
  /// Simple body composition using torque impulse for the windmill blade.
  public class C5Example7 : Node2D, IExample
  {
    public string GetSummary()
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
        rectangleShape2D = new RectangleShape2D { Extents = Extents };
        collisionShape2D = new CollisionShape2D { Shape = rectangleShape2D };
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
        rectangleShape2D = new RectangleShape2D { Extents = Extents };
        collisionShape2D = new CollisionShape2D { Shape = rectangleShape2D };
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
        var windmillBase = new WindmillBase { Extents = BaseExtents };
        AddChild(windmillBase);

        var windmillBlade = new WindmillBlade
        {
          Extents = BladeExtents,
          Torque = BladeTorque,
          Position = windmillBase.Position - new Vector2(0, windmillBase.Extents.y)
        };
        AddChild(windmillBlade);

        var joint = new PinJoint2D
        {
          NodeA = windmillBase.GetPath(),
          NodeB = windmillBlade.GetPath(),
          Softness = 0
        };
        windmillBlade.AddChild(joint);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var windmill = new Windmill
      {
        Position = new Vector2(size.x / 2, size.y - 100)
      };
      AddChild(windmill);

      var spawner = new SimpleTouchSpawner
      {
        SpawnFunction = (position) =>
        {
          return new SimpleBall
          {
            Mass = 0.25f,
            Radius = 10,
            GlobalPosition = position
          };
        }
      };
      AddChild(spawner);
    }
  }
}
