using Godot;

public class C0Exercise5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.5:\n"
      + "Gaussian random walk";
  }

  public class Walker : SimpleWalker
  {
    public override void Step()
    {
      float chance = (float)GD.RandRange(0, 1);
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
