using Godot;

public class C4Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.7:\n"
      + "Particle Repeller";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.SetCreateParticleFunction(() =>
    {
      var particle = new SimpleFallingParticle();
      particle.ParticleMesh = ParticleMeshEnum.Square;
      particle.BodySize = new Vector2(20, 20);
      particle.Lifespan = 2;
      particle.Mass = 2;
      return particle;
    });
    AddChild(particleSystem);

    var repeller = new SimpleRepeller();
    repeller.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(repeller);
  }
}
