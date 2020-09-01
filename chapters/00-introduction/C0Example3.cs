using Godot;
using Drawing;

namespace Examples
{
  /// <summary>
  /// Example 0.3 - Walker moving right.
  /// </summary>
  /// Walker with tuned probabilities to move right more often.
  public class C0Example3 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example I.3:\n"
        + "Walker moving right";
    }

    private class Walker : SimpleWalker
    {
      protected override void Step()
      {
        float chance = (float)GD.RandRange(0, 1);

        if (chance < 0.4)
        {
          x += StepSize;
        }
        else if (chance < 0.6)
        {
          x -= StepSize;
        }
        else if (chance < 0.8)
        {
          y += StepSize;
        }
        else
        {
          y -= StepSize;
        }
      }
    }

    private Walker walker;

    public override void _Ready()
    {
      GD.Randomize();

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
