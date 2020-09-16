using Godot;
using Drawing;

namespace Examples.Chapter3
{
  /// <summary>
  /// Exercise 3.4 - Spiral Path.
  /// </summary>
  /// Uses DrawCanvas and polar coordinates to draw a spiral.
  public class C3Exercise4 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 3.4:\n"
        + "Spiral Path";
    }

    private const float Width = 10;
    private const float Iterations = 20;
    private const float MaxTheta = 200;

    private float theta = 0;
    private float margin = 0;

    public override void _Ready()
    {
      var canvas = new DrawCanvas((pen) =>
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

            pen.DrawCircle((size / 2) + target, Width / 2, MathUtils.RandColor());

            theta += 0.016f;
            margin += 0.016f * 3;
          }
        }
      });
      canvas.QueueClearDrawing(Color.Color8(45, 45, 45));
      AddChild(canvas);
    }
  }
}
