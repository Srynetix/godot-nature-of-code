using Godot;

/**
Example 1.1:
Bouncing ball with no vectors.
*/

public class C1Example1 : Node2D, IExample {
    private float x;
    private float y;
    private float xSpeed = 1f;
    private float ySpeed = 3.3f;

    public string _Summary() {
        return "Example 1.1:\nBouncing ball with no vectors";
    }

    public override void _Draw() {
        DrawCircle(new Vector2(x, y), 20, Colors.Black);
        DrawCircle(new Vector2(x, y), 18, Colors.LightGray);
    }

    public override void _Process(float delta) {
        var size = GetViewport().Size;

        x += xSpeed;
        y += ySpeed;

        if ((x > size.x) || (x < 0)) {
            xSpeed *= -1;
        }

        if ((y > size.y) || (y < 0)) {
            ySpeed *= -1;
        }

        Update();
    }
}
