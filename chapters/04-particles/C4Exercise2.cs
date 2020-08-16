using Godot;

public class C4Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.2:\n"
      + "Particle with Angular Velocity";
  }

  public class EParticle : SimpleParticle
  {
    protected override void UpdateAcceleration()
    {
      Acceleration = new Vector2((float)GD.RandRange(-0.25f, 0.25f), 0.01f);
      AngularAcceleration = Acceleration.x / 10.0f;
    }

    public override void _Draw()
    {
      if (IsDead())
      {
        return;
      }

      var alpha = LifespanAsAlpha ? GetLifespanAlphaValue() : (byte)255;
      DrawRect(new Rect2(-BodySize.x / 2, -BodySize.y / 2, BodySize / 2), Colors.LightBlue.WithAlpha(alpha));
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particle = new EParticle();
    particle.Position = size / 2;
    particle.BodySize = new Vector2(20, 20);
    particle.Lifespan = 4;

    AddChild(particle);
  }
}
