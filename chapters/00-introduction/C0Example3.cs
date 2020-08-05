using Godot;

public class C0Example3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example I.3:\n"
      + "Walker that tends to move to the right";
  }

  public class Walker
  {
    public float x;
    public float y;
    public float StepSize = 3;

    public Walker(Vector2 position)
    {
      x = position.x;
      y = position.y;
    }

    public void Step()
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

  private Walker walker;
  private Utils.Canvas canvas;

  public override void _Ready()
  {
    GD.Randomize();
    walker = new Walker(GetViewport().Size / 2);

    canvas = new Utils.Canvas();
    AddChild(canvas);

    canvas.SetDrawFunction(CanvasDraw);
  }

  public void CanvasDraw(Node2D pen)
  {
    pen.DrawRect(new Rect2(walker.x, walker.y, walker.StepSize, walker.StepSize), Colors.LightCyan, true);
  }

  public override void _Process(float delta)
  {
    walker.Step();
  }
}
