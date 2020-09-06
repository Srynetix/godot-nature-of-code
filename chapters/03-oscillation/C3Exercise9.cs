using Godot;
using Oscillation;

namespace Examples.Chapter3
{
  /// <summary>
  /// Exercise 3.9 - Perlin Wave.
  /// </summary>
  /// Uses a custom SimpleWave with OpenSimplexNoise to draw a perlin wave.
  public class C3Exercise9 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 3.9:\n" +
        "Perlin Wave";
    }

    private class NoiseWave : SimpleWave
    {
      private readonly OpenSimplexNoise noise;

      public NoiseWave()
      {
        noise = new OpenSimplexNoise();
        AngularVelocity = 4f;
        StartAngleFactor = 10f;
      }

      protected override float ComputeY(float angle)
      {
        return MathUtils.Map(noise.GetNoise1d(angle), -1, 1, -Amplitude, Amplitude);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var wave = new NoiseWave
      {
        Separation = 24,
        Length = size.x,
        Position = new Vector2(size.x, size.y) / 2,
        Amplitude = size.y / 2
      };
      AddChild(wave);
    }
  }
}
