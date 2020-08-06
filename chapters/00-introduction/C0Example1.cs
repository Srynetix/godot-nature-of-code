using Godot;

public class C0Example1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example I.1:\n"
      + "Traditional random walk";
  }

  public class Walker : SimpleWalker
  {
    public override void Step()
    {
      var stepX = (float)GD.RandRange(-1.0, 1.0);
      var stepY = (float)GD.RandRange(-1.0, 1.0);

      x += stepX * StepSize;
      y += stepY * StepSize;
    }
  }

  private Walker walker;

  public override void _Ready()
  {
    GD.Randomize();

    walker = new Walker();
    walker.SetXY(GetViewport().Size / 2);
    AddChild(walker);

    var canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    pen.DrawRect(walker.GetStepRect(), Colors.LightCyan, true);
  }
}
