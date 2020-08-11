using Godot;

public class C0Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.4:\n"
      + "Paint splatter";
  }

  private RandomNumberGenerator generator;

  public override void _Ready()
  {
    generator = new RandomNumberGenerator();
    generator.Randomize();

    var canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    var size = GetViewportRect().Size;

    // Follow mouse for fun
    var mousePosition = GetViewport().GetMousePosition();

    float xNum = generator.Randfn(0, 1);  // Gaussian distribution
    float yNum = generator.Randfn(0, 1);  // Gaussian distribution
    var colNumR = (byte)(generator.Randfn(0, 1) * 255);
    var colNumG = (byte)(generator.Randfn(0, 1) * 255);
    var colNumB = (byte)(generator.Randfn(0, 1) * 255);

    float x = 20 * xNum + mousePosition.x;
    float y = 20 * yNum + mousePosition.y;

    pen.DrawCircle(new Vector2(x, y), 8, Color.Color8(colNumR, colNumG, colNumB, 64));
  }
}
