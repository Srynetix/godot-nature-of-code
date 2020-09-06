using Godot;
using Drawing;

namespace Examples.Chapter0
{
  /// <summary>
  /// Exercise 0.6 - Walker with random step.
  /// </summary>
  /// SimpleWalker with randomized step value and direction.
  public class C0Exercise6 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise I.6:\n"
        + "Walker with random step";
    }

    private class Walker : SimpleWalker
    {
      public float ComputeStepSize()
      {
        while (true)
        {
          float prob = (float)GD.RandRange(0, 1);
          float value = (float)GD.RandRange(0, 1);
          float target = value * value;

          if (prob < target)
          {
            return value;
          }
        }
      }

      protected override void Step()
      {
        float stepsize = ComputeStepSize();

        float stepx = (float)GD.RandRange(-stepsize, stepsize);
        float stepy = (float)GD.RandRange(-stepsize, stepsize);

        x += stepx * StepSize;
        y += stepy * StepSize;
      }
    }

    private Walker walker;

    public override void _Ready()
    {
      walker = new Walker();
      walker.SetXY(GetViewportRect().Size / 2);
      AddChild(walker);

      var canvas = new DrawCanvas((pen) => pen.DrawRect(walker.GetStepRect(), Colors.LightCyan, true));
      canvas.QueueClearDrawing(Color.Color8(45, 45, 45));
      AddChild(canvas);
    }
  }
}
