using Godot;
using Drawing;

namespace Examples
{
  /// <summary>
  /// Exercise 0.1 - Walker moving down and right.
  /// </summary>
  /// SimpleWalker with tuned probabilities to go down and right more often.
  public class C0Exercise1 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise I.1:\n"
        + "Walker moving down and right";
    }

    private class Walker : SimpleWalker
    {
      protected override void Step()
      {
        float chance = (float)GD.RandRange(0, 1);

        if (chance < 0.1)
        {
          x -= StepSize;
        }
        else if (chance < 0.2)
        {
          y -= StepSize;
        }
        else if (chance < 0.6)
        {
          x += StepSize;
        }
        else
        {
          y += StepSize;
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
