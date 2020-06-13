using Godot;

/**
Example I.3:
Walker that tends to move to the right
*/

public class C0Example3 : Node2D
{
    public class Walker {
        public float x;
        public float y;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
        }

        public void Step() {
            float chance = GD.Randf();

            if (chance < 0.4) {
                x++;
            } else if (chance < 0.6) {
                x--;
            } else if (chance < 0.8) {
                y++;
            } else {
                y--;
            }
        }
    }   
    
    private Walker walker;
    private Utils.Canvas canvas;

    public override void _Ready() {
        GD.Randomize();
        walker = new Walker(GetViewport().Size / 2);
        VisualServer.SetDefaultClearColor(Colors.White);
        canvas = new Utils.Canvas();
        AddChild(canvas);
    }

    public override void _Process(float delta) {
        walker.Step();

        canvas.Lock();
        canvas.SetPixel((int)walker.x, (int)walker.y, Colors.Black);
        canvas.Unlock();

        canvas.UpdateImage();
    }
}
