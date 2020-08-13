using Godot;

public class SimpleWave : Node2D
{
  public float Radius = 30;
  public int Separation = 24;
  public float AngularVelocity = 0.1f;
  public float StartAngle = 0;
  public float StartAngleFactor = 1;
  public float Length = 300;
  public float Amplitude = 100;

  public virtual float ComputeY(float angle)
  {
    return Utils.Map(Mathf.Sin(angle), -1, 1, -Amplitude, Amplitude);
  }

  public override void _Draw()
  {
    float angle = StartAngle;

    for (float x = -Length / 2; x <= Length / 2; x += Separation)
    {
      var target = new Vector2(x, ComputeY(angle));

      DrawCircle(target, Radius - 2, Colors.White.WithAlpha(100));
      this.DrawCircleOutline(target, Radius, Colors.Gray, 2, 8);
      angle += AngularVelocity;
    }
  }

  public override void _Process(float delta)
  {
    StartAngle += delta * StartAngleFactor;
    Update();
  }
}
