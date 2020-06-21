using Godot;

/**
Example 1.7:
Motion 101 (velocity)
*/

public class C1Example7 : Node2D, IExample {
    public class Mover : Node2D {
        private Vector2 velocity;

        public override void _Ready() {
            var size = GetViewport().Size;
            Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
            velocity = new Vector2((float)GD.RandRange(-2.0f, 2.0f), (float)GD.RandRange(-2.0f, 2.0f));
        }

        public override void _Process(float delta) {
            Position += velocity;
            WrapEdges();
        }

        private void WrapEdges() {
            var size = GetViewport().Size;

            if (Position.x > size.x) {
                Position = new Vector2(0, Position.y);
            } else if (Position.x < 0) {
                Position = new Vector2(size.x, Position.y);
            }

            if (Position.y > size.y) {
                Position = new Vector2(Position.x, 0);
            } else if (Position.y < 0) {
                Position = new Vector2(Position.x, size.y);
            }
        }

        public override void _Draw() {
            DrawCircle(Vector2.Zero, 20, Colors.Black);
            DrawCircle(Vector2.Zero, 18, Colors.LightGray);
        }
    }

    private Mover mover;

    public string _Summary() {
        return "Example 1.7:\nMotion 101 (velocity)";
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Randomize();
        
        mover = new Mover();
        AddChild(mover);
    }
}
