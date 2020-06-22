using Godot;

/**
Example I.3:
Walker that tends to move to the right
*/

public class C0Example3 : Node2D, IExample {
  public class Walker {
    public float x;
    public float y;

    public Walker(Vector2 position) {
      x = position.x;
      y = position.y;
    }

    public void Step() {
      float chance = GD.Randf();

      if (chance < 0.4) {
        x++;
      }
      else if (chance < 0.6) {
        x--;
      }
      else if (chance < 0.8) {
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
    return "Example I.3:\nWalker that tends to move to the right";
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
    walker.Step();
  }
}
