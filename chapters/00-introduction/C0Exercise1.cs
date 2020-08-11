using Godot;

public class C0Exercise1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.1:\n"
      + "Walker moving down and right";
  }

  public class Walker : SimpleWalker
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
