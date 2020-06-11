using Godot;

/**
Example I.1:
Traditional random walk
*/

public class C0Example1 : Node2D
{
    public class Walker {
        float x;
        float y;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
        }

        public void Draw(CanvasItem node) {
            node.DrawCircle(new Vector2(x, y), 1, Colors.Black);
        }

        public void Step() {
            var stepX = (float)GD.RandRange(-1.0, 1.0);
            var stepY = (float)GD.RandRange(-1.0, 1.0);

            x += stepX;
            y += stepY;
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
