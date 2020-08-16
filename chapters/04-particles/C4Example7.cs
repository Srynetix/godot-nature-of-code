using Godot;

public class C4Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.7:\n"
      + "Particle System and Repeller";
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
    repeller.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(repeller);
  }

  public override void _Process(float delta)
  {
    var particle = new EParticle();
    particle.BodySize = new Vector2(10, 10);
    particle.Lifespan = 4;
    particle.Mass = 3;
    particleSystem.AddParticle(particle);
  }
}
