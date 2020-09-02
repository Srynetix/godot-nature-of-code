using Godot;
using Drawing;

/// <summary>
/// Nature of Code examples and exercises.
/// </summary>
namespace Examples
{
  /// <summary>
  /// Chapter 0 - Introduction.
  /// </summary>
  namespace Chapter0
  {
    /// <summary>
    /// Example 0.1 - Traditional random walk.
    /// </summary>
    /// Define a custom walker based on SimpleWalker with a custom step function.
    /// Uses a DrawCanvas to draw without clearing the viewport. 
    public class C0Example1 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example I.1:\n"
          + "Traditional random walk";
      }

      private class Walker : SimpleWalker
      {
        protected override void Step()
        {
          var stepX = (float)GD.RandRange(-1.0, 1.0);
          var stepY = (float)GD.RandRange(-1.0, 1.0);

          x += stepX * StepSize;
          y += stepY * StepSize;
        }
      }

      private Walker walker;

      public override void _Ready()
      {
        walker = new Walker();
        walker.SetXY(GetViewportRect().Size / 2);
        AddChild(walker);

        var canvas = new DrawCanvas((pen) =>
        {
          pen.DrawRect(walker.GetStepRect(), Colors.LightCyan, true);
        });
        canvas.QueueClearDrawing(Color.Color8(45, 45, 45));
        AddChild(canvas);
      }
    }
  }
}
