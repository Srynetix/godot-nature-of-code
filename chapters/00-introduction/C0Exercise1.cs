using Godot;

/**
Exercise I.1:
Create a random walker that has a tendency to move down and to the right.
**/

public class C0Exercise1 : Node2D {
    public class Walker {
        float x;
        float y;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
        }

        public void Draw(CanvasItem node) {
            node.DrawCircle(new Vector2(x, y), 0.5f, Colors.Black);
        }

        public void Step() {
            float chance = GD.Randf();

            if (chance < 0.1) {
                x++;
            } else if (chance < 0.2) {
                y--;
            } else if (chance < 0.6) {
                x--;
            } else {
                y++;
            }
        }
    }

    private Walker walker;

    public override void _Ready() {
        GD.Randomize();
        walker = new Walker(GetViewport().Size / 2);
        VisualServer.SetDefaultClearColor(Colors.White);
        GetViewport().RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
    }

    public override void _Draw() {
        walker.Draw(this);
    }

    public override void _Process(float delta) {
        walker.Step();
        Update();
    }
}
