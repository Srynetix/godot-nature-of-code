using Godot;

/**
Exercise 1.3:
Extend the bouncing ball with vectors example into 3D. Can you get a sphere to bounce around a box?
*/

public class C1Exercise3 : Spatial, IExample
{
    private MeshInstance sphere;
    private Position3D camPoint;

    private Vector3 position;
    private Vector3 velocity = new Vector3(0.75f, 0.5f, 0.25f);

    public string _Summary() {
        return "Exercise 1.3:\nExtend the bouncing ball with vectors example into 3D. Can you get a sphere to bounce around a box?";
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sphere = GetNode<MeshInstance>("Sphere");
        camPoint = GetNode<Position3D>("Position3D");
    }

    public override void _Process(float delta) {
        camPoint.RotateY(delta * 0.5f);
        int limit = 4;
        position += velocity * delta * 10.0f;

        if (position.x > limit || position.x < -limit) {
            velocity.x *= -1;
        }

        if (position.y > limit || position.y < -limit) {
            velocity.y *= -1;
        }

        if (position.z > limit || position.z < -limit) {
            velocity.z *= -1;
        }

        sphere.Transform = new Transform(sphere.Transform.basis, position);
    }
}
