using Godot;
using Drawing;

public class C0Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.9:\n"
      + "Animated 2D noise";
  }

  public class AnimatedNoiseTexture : SimpleNoiseTexture
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
      frameCount += 1;
    }

    protected override float ComputeNoise(float x, float y)
    {
      return noise.GetNoise3d(x, y, time);
    }
  }

  public override void _Ready()
  {
    var noiseTexture = new AnimatedNoiseTexture();
    noiseTexture.Factor = 3;
    noiseTexture.Octaves = 8;
    noiseTexture.NoiseSpeed = 10;
    // Adapt for HTML5
    noiseTexture.AnimationFrameDelay = OS.GetName() == "HTML5" ? 24 : 4;
    AddChild(noiseTexture);
  }
}
