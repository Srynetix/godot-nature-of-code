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
    public string _Summary()
    {
      return "Exercise 3.11:\n"
        + "Combined Wave";
    }

    private class CombinedWave : SimpleWave
    {
      private OpenSimplexNoise noise;

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

      var wave = new CombinedWave();
      wave.Separation = 8;
      wave.StartAngleFactor = 0.75f;
      wave.AngularVelocity = 0.25f;
      wave.Length = size.x;
      wave.Position = new Vector2(size.x, size.y) / 2;
      wave.Amplitude = size.y / 2;
      AddChild(wave);
    }
  }
}
