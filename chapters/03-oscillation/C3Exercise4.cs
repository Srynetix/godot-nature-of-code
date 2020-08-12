using Godot;

public class C3Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.4:\n"
      + "Spiral Path";
  }

  public float Width = 10;
  public float Iterations = 20;
  public float MaxTheta = 200;

  private float theta = 0;
  private float margin = 0;

  private DrawCanvas canvas;

  public override void _Ready()
  {
    GD.Randomize();

    canvas = new DrawCanvas(CanvasDraw);
    canvas.ShowBehindParent = true;
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    // Stop when theta > MaxTheta
    if (theta < MaxTheta)
    {
      for (int i = 0; i < Iterations; ++i)
      {
        float x = (Width + margin) * Mathf.Cos(theta);
        float y = (Width + margin) * Mathf.Sin(theta);
        var target = new Vector2(x, y);
        var size = GetViewportRect().Size;

        pen.DrawCircle(size / 2 + target, Width / 2, Utils.RandColor());

        theta += 0.016f;
        margin += 0.016f * 3;
      }
    }
  }
}
