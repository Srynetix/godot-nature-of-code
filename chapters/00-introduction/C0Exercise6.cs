using Godot;

public class C0Exercise6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.6:\n"
      + "Use a custom probability distribution to vary the size of a step taken by the random walker.\n"
      + "The step size can be determined by influencing the range of values picked.\n"
      + "Can you map the probability exponentially—i.e. making the likelihood that a value is picked equal to the value squared?";
  }

  public class CWalker : Walker
  {
    public float ComputeStepSize()
    {
      while (true)
      {
        float prob = GD.Randf();
        float value = GD.Randf();
        float target = value * value;

        if (prob < target)
        {
          return value;
        }
      }
    }

    public override void Step()
    {
      float stepsize = ComputeStepSize();

      float stepx = (float)GD.RandRange(-stepsize, stepsize);
      float stepy = (float)GD.RandRange(-stepsize, stepsize);

      x += stepx * StepSize;
      y += stepy * StepSize;
    }
  }

  private Walker walker;
  private DrawCanvas canvas;

  public override void _Ready()
  {
    GD.Randomize();
    walker = new CWalker();
    walker.SetXY(GetViewport().Size / 2);

    canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
    AddChild(walker);
  }

  public void CanvasDraw(Node2D pen)
  {
    pen.DrawRect(walker.GetStepRect(), Colors.LightCyan, true);
  }
}
