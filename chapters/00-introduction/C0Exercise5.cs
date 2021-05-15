using Godot;
using Drawing;

namespace Examples.Chapter0
{
    /// <summary>
    /// Exercise 0.5 - Gaussian random walk.
    /// </summary>
    /// SimpleWalker with Gaussian random distribution step.
    public class C0Exercise5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise I.5:\n"
              + "Gaussian random walk";
        }

        private class Walker : SimpleWalker
        {
            protected override void Step()
            {
                var chance = (float)GD.RandRange(0, 1);
                float amount = generator.Randfn(0, 1) * StepSize;  // Gaussian

                if (chance < 0.25)
                {
                    x += amount;
                }
                else if (chance < 0.5)
                {
                    x -= amount;
                }
                else if (chance < 0.75)
                {
                    y += amount;
                }
                else
                {
                    y -= amount;
                }
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
