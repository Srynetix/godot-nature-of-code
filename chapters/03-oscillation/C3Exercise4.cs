using Godot;

public class C3Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.4:\n"
      + "Spiral Path";
  }

  public float Width = 10;
  public float Speed = 8;

  private float theta = 0;
  private float margin = 0;

  private DrawCanvas canvas;

  public override void _Process(float delta)
  {
    theta += delta;
    margin += delta * 2;

    // Let's spin heads!
    canvas.board.RectPivotOffset = canvas.board.RectSize / 2;
    canvas.board.RectRotation += delta * 4;
  }

  public override void _Ready()
  {
    GD.Randomize();

    canvas = new DrawCanvas(CanvasDraw);
    AddChild(canvas);
  }

  public void CanvasDraw(Node2D pen)
  {
    float x = (Width + margin) * Mathf.Cos(theta);
    float y = (Width + margin) * Mathf.Sin(theta);
    var target = new Vector2(x, y);
    var size = GetViewportRect().Size;

    pen.DrawCircle(size / 2 + target, Width / 2, Utils.RandColor());
  }
}
