using Godot;

/**
Exercise I.3:
Create a random walker with dynamic probabilities. 
For example, can you give it a 50% chance of moving in the direction of the mouse?
*/

public class C0Exercise3 : Node2D, IExample {
  public class Walker {
    public float x;
    public float y;

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
          x--;
        }
        else {
          x++;
        }

        if (y > mousePosition.y) {
          y--;
        }
        else {
          y++;
        }
      }
      else {
        RandomStep();
      }
    }

    public void RandomStep() {
      float chance = GD.Randf();

      if (chance < 0.25) {
        x++;
      }
      else if (chance < 0.5) {
        x--;
      }
      else if (chance < 0.75) {
        y++;
      }
      else {
        y--;
      }
    }
  }

  private Walker walker;
  private Utils.Canvas canvas;

  public string _Summary() {
    return "Exercise I.3:\n" +
        "Create a random walker with dynamic probabilities.\n" +
        "For example, can you give it a 50% chance of moving in the direction of the mouse?";
  }

  public override void _Ready() {
    GD.Randomize();
    walker = new Walker(GetViewport().Size / 2);

    canvas = new Utils.Canvas();
    AddChild(canvas);

    canvas.SetDrawFunction(CanvasDraw);
  }

  public void CanvasDraw(Node2D pen) {
    pen.DrawRect(new Rect2(walker.x, walker.y, 1, 1), Colors.LightCyan, true);
  }

  public override void _Process(float delta) {
    walker.Step(this);
  }
}
