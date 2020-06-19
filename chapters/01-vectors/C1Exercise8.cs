using Godot;

/**
Exercise 1.8:
Try implementing the above example with a variable magnitude of acceleration, stronger when it is either closer or farther away.
*/

public class C1Exercise8 : Node2D
{
    public class Mover : Node2D {
        private Vector2 velocity;
        private Vector2 acceleration;
        private float topSpeed;

        public override void _Ready() {
            var size = GetViewport().Size;
            Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            topSpeed = 10;
        }

        public override void _Process(float delta) {
            var mousePos = GetViewport().GetMousePosition();
            var distanceVec = mousePos - Position;
            var distanceLength = distanceVec.Length();
            var dir = distanceVec.Normalized();

            var coef = Mathf.Clamp(1.0f / distanceLength * 10f, 0.25f, 5f);
            acceleration = dir * (1.0f / distanceLength) * 10f;

            velocity += acceleration;
            velocity = velocity.Clamped(topSpeed);
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

    public override void _Ready() {
        GD.Randomize();
        VisualServer.SetDefaultClearColor(Colors.White);
        
        mover = new Mover();
        AddChild(mover);
    }
}
