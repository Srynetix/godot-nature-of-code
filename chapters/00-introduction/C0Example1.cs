using Godot;

/**
Example I.1:
Traditional random walk
*/

public class C0Example1 : Node2D
{
    public class Walker {
        public float x;
        public float y;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
        }

        public void Step() {
            var stepX = (float)GD.RandRange(-1.0, 1.0);
            var stepY = (float)GD.RandRange(-1.0, 1.0);

            x += stepX;
            y += stepY;
        }
    }   
    
    private Walker walker;
    private Utils.Canvas canvas;

    public override void _Ready() {
        var size = GetViewport().Size;
        GD.Randomize();
        walker = new Walker(size / 2);
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
