using Godot;

public class C4Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.9:\n"
      + "Particle System Multiple Repellers";
  }

  public class EParticle : SimpleParticle
  {
    protected override void UpdateAcceleration()
    {
      ApplyForce(new Vector2((float)GD.RandRange(-0.1f, 0.1f), 0.15f));
    }
  }

  private SimpleParticleSystem particleSystem;

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    particleSystem = new SimpleParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(particleSystem);

    var repeller = new SimpleRepeller();
    repeller.Position = particleSystem.Position + new Vector2(0, 100);
    AddChild(repeller);

    var repeller2 = new SimpleRepeller();
    repeller2.Position = particleSystem.Position + new Vector2(-100, 0);
    AddChild(repeller2);
  }

  public override void _Process(float delta)
  {
    var particle = new EParticle();
    particle.BodySize = new Vector2(10, 10);
    particle.Lifespan = 4;
    particle.Mass = 1;
    particleSystem.AddParticle(particle);
  }
}
