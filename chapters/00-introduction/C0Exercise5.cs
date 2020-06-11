using Godot;

/**
Exercise I.5:
A Gaussian random walk is defined as one in which the step size (how far the object moves in a given direction) is generated with a normal distribution.
Implement this variation of our random walk.
*/

public class C0Exercise5 : Node2D
{
    public class Walker {
        float x;
        float y;
        RandomNumberGenerator generator;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
            generator = new RandomNumberGenerator();
            generator.Randomize();
        }

        public void Draw(CanvasItem node) {
            node.DrawCircle(new Vector2(x, y), 0.5f, Colors.Black);
        }

        public void Step(CanvasItem node) {
            RandomStep();
        }

        public void RandomStep() {
            float chance = GD.Randf();
            float amount = generator.Randfn(0, 1);  // Gaussian

            if (chance < 0.25) {
                x += amount;
            } else if (chance < 0.5) {
                x -= amount;
            } else if (chance < 0.75) {
                y += amount;
            } else {
                y -= amount;
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
