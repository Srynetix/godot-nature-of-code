using Godot;

public class C4Exercise10 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.9:\n"
      + "Responding Particles";
  }

  public class EParticle : SimpleParticle
  {
    public override void _Ready()
    {
      base._Ready();

      // Every particle should attract each other
      var attractor = new SimpleAttractor();
      attractor.Gravitation = 1f;
      attractor.Drawing = false;
      AddChild(attractor);
    }

    protected override void UpdateAcceleration()
    {
      ApplyForce(new Vector2((float)GD.RandRange(-0.1f, 0.1f), 0.015f));
    }
  }

  public class EParticleSystem : SimpleParticleSystem
  {
    public override void _Ready()
    {
      base._Ready();
      SetCreateParticleFunction(CreateParticle);
    }

    public SimpleParticle CreateParticle()
    {
      var particle = new EParticle();
      particle.BodySize = new Vector2(10, 10);
      particle.Lifespan = 4;
      particle.Mass = 4;
      return particle;
    }
  }

  private SimpleParticleSystem particleSystem;

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new EParticleSystem();
    particleSystem.ParticleSpawnFrameDelay = 4;
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.Emitting = true;
    AddChild(particleSystem);
  }
}
