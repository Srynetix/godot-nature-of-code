using Godot;

public class C4Example3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.3:\n"
      + "Particle System";
  }

  public class EParticle : SimpleSquareParticle
  {
    protected override void UpdateAcceleration()
    {
      AngularAcceleration = Acceleration.x / 10f;
      ApplyForce(new Vector2((float)GD.RandRange(-0.5f, 0.5f), 0.15f));
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
      particle.Lifespan = 2;
      return particle;
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new EParticleSystem();
    particleSystem.ParticleSpawnFrameDelay = 0;
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.Emitting = true;
    AddChild(particleSystem);
  }
}
