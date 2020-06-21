using Godot;

/**
Example 1.2:
Bouncing ball with vectors.
*/

public class C1Example2 : Node2D, IExample {
    private Vector2 position = new Vector2(100, 100);
    private Vector2 velocity = new Vector2(2.5f, 5f);

    public string _Summary() {
        return "Example 1.2:\n" +
            "Bouncing ball with vectors.";
    }

    public override void _Draw() {
        DrawCircle(position, 20, Colors.Black);
        DrawCircle(position, 18, Colors.LightGray);
    }

    public override void _Process(float delta) {
        var size = GetViewport().Size;

        position += velocity;

        if ((position.x > size.x) || (position.x < 0)) {
            velocity.x *= -1;
        }

        if ((position.y > size.y) || (position.y < 0)) {
            velocity.y *= -1;
        }

        Update();
    }
}
