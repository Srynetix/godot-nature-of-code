using Godot;

public class SimpleFallingParticle : SimpleParticle
{
  public Vector2 ForceRangeX = new Vector2(-0.15f, 0.15f);
  public Vector2 ForceRangeY = new Vector2(0.15f, 0.15f);

  protected override void UpdateAcceleration()
  {
    ApplyForce(Utils.RandVector2(ForceRangeX, ForceRangeY));
    AngularAcceleration = Acceleration.x / 10f;
  }
}
