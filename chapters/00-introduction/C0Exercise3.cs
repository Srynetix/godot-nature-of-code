using Godot;

public class C0Exercise3 : Node2D, IExample {
  public string _Summary() {
    return "Exercise I.3:\n"
      + "Create a random walker with dynamic probabilities.\n"
      + "For example, can you give it a 50% chance of moving in the direction of the mouse?";
  }

  public class Walker {
    public float x;
    public float y;
    public float StepSize = 3;

    public Walker(Vector2 position) {
      x = position.x;
      y = position.y;
    }

    public void Step(CanvasItem node) {
      float chance = GD.Randf();

      if (chance <= 0.5) {
        // Go towards mouse
        var mousePosition = node.GetViewport().GetMousePosition();
        if (x > mousePosition.x) {
          x -= StepSize;
        }
        else {
          x += StepSize;
        }

        if (y > mousePosition.y) {
          y -= StepSize;
        }
        else {
          y += StepSize;
        }
      }
      else {
        RandomStep();
      }
    }

    public void RandomStep() {
      float chance = GD.Randf();

      if (chance < 0.25) {
        x += StepSize;
      }
      else if (chance < 0.5) {
        x -= StepSize;
      }
      else if (chance < 0.75) {
        y += StepSize;
      }
      else {
        y -= StepSize;
      }
    }
  }

  private Walker walker;
  private Utils.Canvas canvas;

  public override void _Ready() {
    GD.Randomize();
    walker = new Walker(GetViewport().Size / 2);

    canvas = new Utils.Canvas();
    AddChild(canvas);

    canvas.SetDrawFunction(CanvasDraw);
  }

  public void CanvasDraw(Node2D pen) {
    pen.DrawRect(new Rect2(walker.x, walker.y, walker.StepSize, walker.StepSize), Colors.LightCyan, true);
  }

  public override void _Process(float delta) {
    walker.Step(this);
  }
}
