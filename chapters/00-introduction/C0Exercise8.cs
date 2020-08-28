using Godot;
using Drawing;

public class C0Exercise8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.8:\n"
      + "Noise visual effects";
  }

  public class NoiseTextureEffects : SimpleNoiseTexture
  {
    protected override float ComputeNoise(float x, float y)
    {
      return noise.GetNoise2d(x + (float)GD.RandRange(0, 1) * 10, y + (float)GD.RandRange(0, 1) * 10);
    }
  }

  public override void _Ready()
  {
    var noiseTexture = new NoiseTextureEffects();
    noiseTexture.Factor = 3;
    noiseTexture.Octaves = 8;
    AddChild(noiseTexture);
  }
}
