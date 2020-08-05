using Godot;

public class C0Example3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example I.3:\n"
      + "Walker that tends to move to the right";
  }

  public class CWalker : Walker
  {
    public override void Step()
    {
      float chance = GD.Randf();

      if (chance < 0.4)
      {
        x += StepSize;
      }
      else if (chance < 0.6)
      {
        x -= StepSize;
      }
      else if (chance < 0.8)
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
