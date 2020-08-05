using Godot;

public class C0Exercise1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.1:\n"
      + "Create a random walker that has a tendency to move down and to the right";
  }

  public class CWalker : Walker
  {
    public override void Step()
    {
      float chance = GD.Randf();

      if (chance < 0.1)
      {
        x -= StepSize;
      }
      else if (chance < 0.2)
      {
        y -= StepSize;
      }
      else if (chance < 0.6)
      {
        x += StepSize;
      }
      else
      {
        y += StepSize;
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
