using Godot;
using Oscillation;

public class C3Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.9:\n" +
      "Perlin Wave";
  }

  public class NoiseWave : SimpleWave
  {
    private OpenSimplexNoise noise;

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

  public override void _Draw()
  {
    var size = GetViewportRect().Size;

    var wave = new NoiseWave();
    wave.Separation = 24;
    wave.Length = size.x;
    wave.Position = new Vector2(size.x, size.y) / 2;
    wave.Amplitude = size.y / 2;
    AddChild(wave);
  }
}
