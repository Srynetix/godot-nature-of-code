using Godot;

/**
Exercise I.5:
A Gaussian random walk is defined as one in which the step size (how far the object moves in a given direction) is generated with a normal distribution.
Implement this variation of our random walk.
*/

public class C0Exercise5 : Node2D, IExample {
  public class Walker {
    public float x;
    public float y;
    RandomNumberGenerator generator;

    public Walker(Vector2 position) {
      x = position.x;
      y = position.y;
      generator = new RandomNumberGenerator();
      generator.Randomize();
    }

    public void Step(CanvasItem node) {
      RandomStep();
    }

    public void RandomStep() {
      float chance = GD.Randf();
      float amount = generator.Randfn(0, 1);  // Gaussian

      if (chance < 0.25) {
        x += amount;
      }
      else if (chance < 0.5) {
        x -= amount;
      }
      else if (chance < 0.75) {
        y += amount;
      }
      else {
        y -= amount;
      }
    }
  }

  private Walker walker;
  private Utils.Canvas canvas;

  public string _Summary() {
    return "Exercise I.5:\n" +
        "A Gaussian random walk is defined as one in which the step size (how far the object moves in a given direction) is generated with a normal distribution.\n" +
        "Implement this variation of our random walk.";
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
