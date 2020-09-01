using Godot;
using Drawing;

namespace Examples
{
  namespace Chapter0
  {
    /// <summary>
    /// Example 0.4 - Gaussian distribution.
    /// </summary>
    /// Uses DrawCanvas to draw circles, and Godot's RandomNumberGenerator.
    public class C0Example4 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example I.4:\n"
          + "Gaussian distribution";
      }

      private RandomNumberGenerator generator;

      public override void _Ready()
      {
        generator = new RandomNumberGenerator();
        generator.Randomize();

        var canvas = new DrawCanvas((pen) =>
        {
          var size = GetViewportRect().Size;
          float num = generator.Randfn(0, 1);  // Gaussian distribution
          float sd = size.x / 8;
          float mean = size.x / 2;
          float x = sd * num + mean;
          pen.DrawCircle(new Vector2(x, size.y / 2), 8, Colors.LightCyan.WithAlpha(10));
        });
        canvas.QueueClearDrawing(Color.Color8(45, 45, 45));
        AddChild(canvas);
      }
    }
  }
}
