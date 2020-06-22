using Godot;

/**
Example I.4:
Gaussian distribution
*/

public class C0Example4 : Node2D, IExample {
  private RandomNumberGenerator generator;
  private Utils.Canvas canvas;

  public string _Summary() {
    return "Example I.4:\nGaussian distribution";
  }

  public override void _Ready() {
    generator = new RandomNumberGenerator();
    generator.Randomize();

    canvas = new Utils.Canvas();
    AddChild(canvas);

    canvas.SetDrawFunction(CanvasDraw);
  }

  public void CanvasDraw(Node2D pen) {
    var size = GetViewport().Size;

    float num = generator.Randfn(0, 1);  // Gaussian distribution
    float sd = size.x / 8;
    float mean = size.x / 2;

    float x = sd * num + mean;

    pen.DrawCircle(new Vector2(x, size.y / 2), 8, Colors.LightCyan.WithAlpha(10));
  }
}
