using Godot;

namespace Examples
{
  namespace Chapter1
  {
    /// <summary>
    /// Exercise 1.3 - 3D bouncing ball.
    /// </summary>
    /// Uses simple limit detection at an arbitrary length.
    public class C1Exercise3 : Spatial, IExample
    {
      public string _Summary()
      {
        return "Exercise 1.3:\n"
          + "3D bouncing ball";
      }

      private MeshInstance sphere;
      private Position3D camPoint;

      private Vector3 position;
      private Vector3 velocity = new Vector3(0.75f, 0.5f, 0.25f);

      // Called when the node enters the scene tree for the first time.
      public override void _Ready()
      {
        sphere = GetNode<MeshInstance>("Sphere");
        camPoint = GetNode<Position3D>("Position3D");
      }

      public override void _Process(float delta)
      {
        camPoint.RotateY(delta * 0.5f);
        int limit = 4;
        position += velocity * delta * 10.0f;

        if (position.x > limit || position.x < -limit)
        {
          velocity.x *= -1;
        }

        if (position.y > limit || position.y < -limit)
        {
          velocity.y *= -1;
        }

        if (position.z > limit || position.z < -limit)
        {
          velocity.z *= -1;
        }

        sphere.Transform = new Transform(sphere.Transform.basis, position);
      }
    }
  }
}
