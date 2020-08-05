using Godot;

public class C0Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example I.4:\n"
      + "Gaussian distribution";
  }

  private RandomNumberGenerator generator;
  private DrawCanvas canvas;

  public override void _Ready()
  {
    generator = new RandomNumberGenerator();
    generator.Randomize();

    canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    var size = GetViewport().Size;

    float num = generator.Randfn(0, 1);  // Gaussian distribution
    float sd = size.x / 8;
    float mean = size.x / 2;

    float x = sd * num + mean;

    pen.DrawCircle(new Vector2(x, size.y / 2), 8, Colors.LightCyan.WithAlpha(10));
  }
}
