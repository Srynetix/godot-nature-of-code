using Godot;

public class C4Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.9:\n"
      + "Particle System Multiple Repellers";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.SetCreateParticleFunction(CreateParticle);
    AddChild(particleSystem);

    var repeller = new SimpleRepeller();
    repeller.Position = particleSystem.Position + new Vector2(0, 100);
    AddChild(repeller);

    var repeller2 = new SimpleRepeller();
    repeller2.Position = particleSystem.Position + new Vector2(-100, 0);
    AddChild(repeller2);
  }

  private SimpleParticle CreateParticle()
  {
    var particle = new SimpleFallingParticle();
    particle.BodySize = new Vector2(10, 10);
    particle.Lifespan = 2;
    particle.Mass = 2;
    return particle;
  }
}
