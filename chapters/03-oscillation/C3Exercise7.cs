using Godot;
using System.Linq;
using Oscillation;

namespace Examples.Chapter3
{
    /// <summary>
    /// Exercise 3.7 - Controlled oscillators.
    /// </summary>
    /// Uses multiple SimpleOscillators.
    public class C3Exercise7 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 3.7:\n"
              + "Controlled oscillators";
        }

        public override void _Ready()
        {
            const int oscillatorsCount = 10;
            var size = GetViewportRect().Size;
            var sizeOffset = size.x / oscillatorsCount;

            foreach (int i in Enumerable.Range(0, oscillatorsCount))
            {
                var oscillator = new SimpleOscillator
                {
                    Amplitude = new Vector2(24, size.y / 4),
                    Velocity = new Vector2(0.01f, 0.025f * (i + 1)),
                    Position = new Vector2((sizeOffset / 2) + (sizeOffset * i), size.y / 2)
                };
                AddChild(oscillator);
            }
        }
    }
}
