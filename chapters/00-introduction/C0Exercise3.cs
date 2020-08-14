using Godot;

public class C0Exercise3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.3:\n"
      + "Walker 50% moving to mouse";
  }

  public class Walker : SimpleWalker
  {
    public override void Step()
    {
      float chance = (float)GD.RandRange(0, 1);

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
      float chance = (float)GD.RandRange(0, 1);

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
