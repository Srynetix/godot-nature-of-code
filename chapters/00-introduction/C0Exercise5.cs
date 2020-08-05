using Godot;

public class C0Exercise5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.5:\n"
      + "A Gaussian random walk is defined as one in which the step size (how far the object moves in a given direction) is generated with a normal distribution.\n"
      + "Implement this variation of our random walk.";
  }

  public class CWalker : Walker
  {
    public override void Step()
    {
      float chance = GD.Randf();
      float amount = generator.Randfn(0, 1) * StepSize;  // Gaussian

      if (chance < 0.25)
      {
        x += amount;
      }
      else if (chance < 0.5)
      {
        x -= amount;
      }
      else if (chance < 0.75)
      {
        y += amount;
      }
      else
      {
        y -= amount;
      }
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
