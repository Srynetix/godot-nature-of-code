using Godot;

public class C4Exercise3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.3:\n"
      + "Dynamic Particle System";
  }

  public class DynamicParticleSystem : SimpleParticleSystem
  {
    public DynamicParticleSystem() : base(WrapModeEnum.Bounce)
    {
      LocalCoords = false;
      DisableForces = false;
    }

    protected override void UpdateAcceleration()
    {
      float offset = 0.5f;
      ApplyForce(new Vector2((float)GD.RandRange(-offset, offset), (float)GD.RandRange(-offset, offset)));
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new DynamicParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.ParticleSpawnFrameDelay = 1;
    particleSystem.SetCreateParticleFunction(() =>
    {
      var particle = new SimpleFallingParticle();
      particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
      particle.ParticleMesh = ParticleMeshEnum.Square;
      particle.BodySize = new Vector2(20, 20);
      particle.Lifespan = 2;
      return particle;
    });
    AddChild(particleSystem);
  }
}
