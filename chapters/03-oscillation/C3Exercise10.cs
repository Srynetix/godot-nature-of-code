using Godot;
using Oscillation;

namespace Examples.Chapter3
{
  /// <summary>
  /// Exercise 3.10 - Multiple Waves.
  /// <summary>
  /// Display multiple wave types using SimpleWave.
  public class C3Exercise10 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 3.10:\n"
        + "Multiple Waves";
    }

    private class NoiseWave : SimpleWave
    {
      private OpenSimplexNoise noise;

      public NoiseWave()
      {
        noise = new OpenSimplexNoise();
      }

      protected override float ComputeY(float angle)
      {
        return MathUtils.Map(noise.GetNoise1d(angle * 10f), -1, 1, -Amplitude, Amplitude);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var wave1 = new SimpleWave();
      wave1.Separation = 8;
      wave1.Length = 150;
      wave1.Position = new Vector2(size.x / 8, size.y / 4);
      wave1.Amplitude = 75;
      AddChild(wave1);

      var wave2 = new NoiseWave();
      wave2.Separation = 4;
      wave2.Length = 300;
      wave2.Position = new Vector2(size.x / 2, size.y / 2);
      wave2.Amplitude = 100;
      wave2.AngularVelocity = 0.25f;
      AddChild(wave2);

      var wave3 = new SimpleWave();
      wave3.Separation = 8;
      wave3.StartAngleFactor = 10;
      wave3.Length = 150;
      wave3.Position = new Vector2(size.x - size.x / 8, size.y - size.y / 4);
      wave3.Amplitude = 50;
      AddChild(wave3);
    }
  }
}
