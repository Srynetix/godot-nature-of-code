using Godot;
using Drawing;

namespace Examples
{
  namespace Chapter0
  {
    /// <summary>
    /// Exercise 0.3 - Walker 50% moving to mouse.
    /// </summary>
    /// SimpleWalker with tuned probabilities to mostly go towards mouse position.
    public class C0Exercise3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise I.3:\n"
          + "Walker 50% moving to mouse";
      }

      private class Walker : SimpleWalker
      {
        protected override void Step()
        {
          float chance = (float)GD.RandRange(0, 1);

          if (chance <= 0.5)
          {
            // Go towards mouse
            var mousePosition = GetViewport().GetMousePosition();
            if (x > mousePosition.x)
            {
              x -= StepSize;
            }
            else
            {
              x += StepSize;
            }

            if (y > mousePosition.y)
            {
              y -= StepSize;
            }
            else
            {
              y += StepSize;
            }
          }
          else
          {
            RandomStep();
          }
        }

        public void RandomStep()
        {
          float chance = (float)GD.RandRange(0, 1);

          if (chance < 0.25)
          {
            x += StepSize;
          }
          else if (chance < 0.5)
          {
            x -= StepSize;
          }
          else if (chance < 0.75)
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
}
