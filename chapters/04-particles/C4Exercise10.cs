using Godot;

public class C4Exercise10 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.10:\n"
      + "Responding Particles";
  }

  public class EParticle : SimpleFallingParticle
  {
    public override void _Ready()
    {
      base._Ready();
      WrapMode = WrapModeEnum.Bounce;
      ForceRangeX = new Vector2(-0.1f, 0.1f);
      ForceRangeY = new Vector2(0.005f, 0.005f);

      // Every particle should attract each other
      var attractor = new SimpleAttractor();
      attractor.Drawing = false;
      AddChild(attractor);
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.SetCreateParticleFunction(() =>
    {
      var particle = new EParticle();
      particle.BodySize = new Vector2(20, 20);
      particle.ParticleMesh = ParticleMeshEnum.Square;
      particle.Lifespan = 4;
      particle.Mass = 4;
      return particle;
    });
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(particleSystem);
  }
}
