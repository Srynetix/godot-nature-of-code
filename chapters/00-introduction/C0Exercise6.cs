using Godot;

/**
Exercise I.5:
Use a custom probability distribution to vary the size of a step taken by the random walker.
The step size can be determined by influencing the range of values picked.
Can you map the probability exponentially—i.e. making the likelihood that a value is picked equal to the value squared?
*/

public class C0Exercise6 : Node2D
{
    public class Walker {
        public float x;
        public float y;

        public Walker(Vector2 position) {
            x = position.x;
            y = position.y;
        }

        public void Step(CanvasItem node) {
            RandomStep();
        }

        public float ComputeStepSize() {
            while (true) {
                float prob = GD.Randf();
                float value = GD.Randf();
                float target = value * value;

                if (prob < target) {
                    return value;
                }
            }
        }

        public void RandomStep() {
            float stepsize = ComputeStepSize();

            float stepx = (float)GD.RandRange(-stepsize, stepsize);
            float stepy = (float)GD.RandRange(-stepsize, stepsize);
                    
            x += stepx;
            y += stepy;
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
        walker.Step(this);

        canvas.Lock();
        canvas.SetPixel((int)walker.x, (int)walker.y, Colors.Black);
        canvas.Unlock();

        canvas.UpdateImage();
    }
}
