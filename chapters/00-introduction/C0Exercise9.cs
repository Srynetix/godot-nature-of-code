using Godot;
using Drawing;

namespace Examples.Chapter0
{
    /// <summary>
    /// Exercise 0.9 - Animated 2D noise.
    /// </summary>
    /// Dynamic 2D noise based on SimpleNoiseTexture.
    /// Can be slow on mobile and HTML5 environments.
    public class C0Exercise9 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise I.9:\n"
              + "Animated 2D noise";
        }

        private class AnimatedNoiseTexture : SimpleNoiseTexture
        {
            public float NoiseSpeed = 1;
            public int AnimationFrameDelay = 1;

            private float time;
            private float frameCount;

            public override void _Process(float delta)
            {
                base._Process(delta);

                if (frameCount == AnimationFrameDelay)
                {
                    GenerateNoiseTexture();
                    texture.SetData(image);
                    frameCount = 0;
                }

                time += delta * NoiseSpeed;
                frameCount++;
            }

            protected override float ComputeNoise(float x, float y)
            {
                return noise.GetNoise3d(x, y, time);
            }
        }

        public override void _Ready()
        {
            var noiseTexture = new AnimatedNoiseTexture { Factor = 3, Octaves = 8, NoiseSpeed = 10 };
            // Adapt for HTML5
            noiseTexture.AnimationFrameDelay = OS.GetName() == "HTML5" ? 24 : 4;
            AddChild(noiseTexture);
        }
    }
}
