using Godot;

public class C4Exercise3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.3:\n"
      + "Dynamic Particle System";
  }

  public class EParticle : SimpleSquareParticle
  {
    protected override void UpdateAcceleration()
    {
      AngularAcceleration = Acceleration.x / 10f;
      ApplyForce(new Vector2((float)GD.RandRange(-0.5f, 0.5f), 0.15f));
    }
  }

  public class DynamicParticleSystem : SimpleParticleSystem
  {
    public DynamicParticleSystem() : base(WrapModeEnum.Bounce)
    {
      DisableForces = false;
    }

    protected override void UpdateAcceleration()
    {
      float offset = 0.5f;
      ApplyForce(new Vector2((float)GD.RandRange(-offset, offset), (float)GD.RandRange(-offset, offset)));
    }
  }

  private SimpleParticleSystem particleSystem;

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    particleSystem = new DynamicParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(particleSystem);
  }

  public override void _Process(float delta)
  {
    var particle = new EParticle();
    particle.BodySize = new Vector2(10, 10);
    particle.Lifespan = 2;
    particleSystem.AddParticle(new EParticle());
  }
}
