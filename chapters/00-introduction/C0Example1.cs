using Godot;

public class C0Example1 : Node2D, IExample {
  public string _Summary() {
    return "Example I.1:\n"
      + "Traditional random walk";
  }

  public class Walker {
    public float x;
    public float y;

    public Walker(Vector2 position) {
      x = position.x;
      y = position.y;
    }

    public void Step() {
      var stepX = (float)GD.RandRange(-1.0, 1.0);
      var stepY = (float)GD.RandRange(-1.0, 1.0);

      x += stepX;
      y += stepY;
    }
  }

  private Walker walker;
  private Utils.Canvas canvas;

  public override void _Ready() {
    var size = GetViewport().Size;
    GD.Randomize();
    walker = new Walker(size / 2);

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
