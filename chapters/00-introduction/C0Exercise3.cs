using Godot;

/**
Exercise I.3:
Create a random walker with dynamic probabilities. 
For example, can you give it a 50% chance of moving in the direction of the mouse?
*/

public class C0Exercise3 : Node2D
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

        public void Step(CanvasItem node) {
            float chance = GD.Randf();

            if (chance <= 0.5) {
                // Go towards mouse
                var mousePosition = node.GetViewport().GetMousePosition();
                if (x > mousePosition.x) {
                    x--;
                } else {
                    x++;
                }

                if (y > mousePosition.y) {
                    y--;
                } else {
                    y++;
                }
            } else {
                RandomStep();
            }
        }

        public void RandomStep() {
            float chance = GD.Randf();

            if (chance < 0.25) {
                x++;
            } else if (chance < 0.5) {
                x--;
            } else if (chance < 0.75) {
                y++;
            } else {
                y--;
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
        walker.Step(this);
        Update();
    }
}
