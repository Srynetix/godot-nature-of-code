using Godot;

/**
Example 1.7:
Motion 101 (velocity)
*/

public class C1Example7 : Node2D {
    public class Mover {
        private Vector2 position;
        private Vector2 velocity;

        public Mover(Vector2 size) {
            position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
            velocity = new Vector2((float)GD.RandRange(-2.0f, 2.0f), (float)GD.RandRange(-2.0f, 2.0f));
        }

        public void Update(Vector2 size) {
            position += velocity;
            WrapEdges(size);
        }

        private void WrapEdges(Vector2 size) {
            if (position.x > size.x) {
                position.x = 0;
            } else if (position.x < 0) {
                position.x = size.x;
            }

            if (position.y > size.y) {
                position.y = 0;
            } else if (position.y < 0) {
                position.y = size.y;
            }
        }

        public void Draw(Node2D node) {
            node.DrawCircle(position, 20, Colors.Black);
            node.DrawCircle(position, 18, Colors.LightGray);
        }
    }

    private Mover mover;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GD.Randomize();
        VisualServer.SetDefaultClearColor(Colors.White);
        
        mover = new Mover(GetViewport().Size);
    }

    public override void _Process(float delta) {
        mover.Update(GetViewport().Size);
        Update();
    }

    public override void _Draw() {
        mover.Draw(this);
    }
}
