using Godot;
using Drawing;

namespace Examples.Chapter0
{
    /// <summary>
    /// Example 0.6 - 2D perlin noise.
    /// </summary>
    /// Generate a noise texture with SimpleNoiseTexture.
    public class C0Example6 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example I.6:\n"
              + "2D Perlin noise";
        }

        public override void _Ready()
        {
            var noiseTexture = new SimpleNoiseTexture()
            {
                Factor = 3,
                Octaves = 8
            };
            AddChild(noiseTexture);
        }
    }
}
