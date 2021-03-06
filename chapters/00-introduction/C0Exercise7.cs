using Godot;
using Drawing;

namespace Examples.Chapter0
{
    /// <summary>
    /// Exercise 0.7 - Walker with noise step.
    /// </summary>
    /// SimpleWalker with randomized step value using OpenSimplexNoise.
    public class C0Exercise7 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise I.7:\n"
              + "Walker with noise step";
        }

        private class Walker : SimpleWalker
        {
            public override void _Draw()
            {
                DrawCircle(Vector2.Zero, 20, Colors.Black);
                DrawCircle(Vector2.Zero, 18, Colors.LightGray);
            }

            protected override void Step()
            {
                RandomStep();

                tx++;
                ty++;
            }

            public float ComputeStepSize(float t)
            {
                return MathUtils.Map(noise.GetNoise1d(t), -1, 1, -1, 1);
            }

            public void RandomStep()
            {
                float stepx = ComputeStepSize(tx);
                float stepy = ComputeStepSize(ty);

                x += stepx;
                y += stepy;
            }
        }

        private Walker walker;

        public override void _Ready()
        {
            walker = new Walker();
            walker.SetXY(GetViewportRect().Size / 2);
            AddChild(walker);
        }
    }
}
