using Godot;

/**
Exercise I.7:
In the above random walker, the result of the noise function is mapped directly to the Walker’s location.
Create a random walker where you instead map the result of the noise() function to a Walker’s step size.
*/

public class C0Exercise7 : Node2D
{
    public class Walker {
        float x;
        float y;
        float tx;
        float ty;
        OpenSimplexNoise noise;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
            noise = new OpenSimplexNoise();
            tx = 0;
            ty = 10000;
        }

        public void Draw(CanvasItem node) {
            node.DrawCircle(new Vector2(x, y), 20, Colors.Black);
            node.DrawCircle(new Vector2(x, y), 18, Colors.LightGray);
        }

        public void Step(CanvasItem node) {
            RandomStep();

            tx += 1f;
            ty += 1f;
        }

        public float ComputeStepSize(float t) {
            return Utils.Map(noise.GetNoise1d(t), -1, 1, -1, 1);
        }

        public void RandomStep() {
            float stepx = ComputeStepSize(tx);
            float stepy = ComputeStepSize(ty);

            x += stepx;
            y += stepy;
        }
    }   
    
    private Walker walker;

    public override void _Ready() {
        GD.Randomize();
        walker = new Walker(GetViewport().Size / 2);
        VisualServer.SetDefaultClearColor(Colors.White);
    }
    
    public override void _Draw() {
        walker.Draw(this);
    }

    public override void _Process(float delta) {
        walker.Step(this);
        Update();
    }
}
