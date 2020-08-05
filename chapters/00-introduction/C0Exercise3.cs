using Godot;

public class C0Exercise3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.3:\n"
      + "Create a random walker with dynamic probabilities.\n"
      + "For example, can you give it a 50% chance of moving in the direction of the mouse?";
  }

  public class CWalker : Walker
  {
    public override void Step()
    {
      float chance = GD.Randf();

      if (chance <= 0.5)
      {
        // Go towards mouse
        var mousePosition = GetViewport().GetMousePosition();
        if (x > mousePosition.x)
        {
          x -= StepSize;
        }
        else
        {
          x += StepSize;
        }

        if (y > mousePosition.y)
        {
          y -= StepSize;
        }
        else
        {
          y += StepSize;
        }
      }
      else
      {
        RandomStep();
      }
    }

    public void RandomStep()
    {
      float chance = GD.Randf();

      if (chance < 0.25)
      {
        x += StepSize;
      }
      else if (chance < 0.5)
      {
        x -= StepSize;
      }
      else if (chance < 0.75)
      {
        y += StepSize;
      }
      else
      {
        y -= StepSize;
      }
    }
  }

  private CWalker walker;
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
