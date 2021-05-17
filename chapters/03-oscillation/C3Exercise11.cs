using Godot;
using Oscillation;

namespace Examples.Chapter3
{
    /// <summary>
    /// Exercise 3.11 - Combined Wave.
    /// </summary>
    /// Uses a custom ComputeY implementation of SimpleWave to combine waves.
    public class C3Exercise11 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 3.11:\n"
              + "Combined Wave";
        }

        private class CombinedWave : SimpleWave
        {
            private readonly OpenSimplexNoise noise;

            public CombinedWave()
            {
                noise = new OpenSimplexNoise();
            }

            protected override float ComputeY(float angle)
            {
                var n1 = MathUtils.Map(noise.GetNoise1d(angle * 10f), -1, 1, -Amplitude * 0.75f, Amplitude * 0.75f);
                var n2 = MathUtils.Map(Mathf.Sin(angle), -1, 1, -Amplitude * 0.25f, Amplitude * 0.25f);
                return n1 + n2;
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var wave = new CombinedWave()
            {
                Separation = 8,
                StartAngleFactor = 0.75f,
                AngularVelocity = 0.25f,
                Length = size.x,
                Position = new Vector2(size.x, size.y) / 2,
                Amplitude = size.y / 2
            };
            AddChild(wave);
        }
    }
}
