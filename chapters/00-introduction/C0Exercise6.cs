using Godot;

public class C0Exercise6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.6:\n"
      + "Walker with random step";
  }

  public class Walker : SimpleWalker
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

  public override void _Ready()
  {
    GD.Randomize();
    walker = new Walker();
    walker.SetXY(GetViewportRect().Size / 2);
    AddChild(walker);

    var canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    pen.DrawRect(walker.GetStepRect(), Colors.LightCyan, true);
  }
}
