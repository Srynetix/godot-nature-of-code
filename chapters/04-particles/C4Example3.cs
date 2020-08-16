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
    protected override void InitializeParticle(SimpleParticle particle)
    {
      particle.BodySize = new Vector2(10, 10);
    }
  }

  private SimpleParticleSystem particleSystem;

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    particleSystem = new SimpleParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(particleSystem);
  }

  public override void _Process(float delta)
  {
    particleSystem.AddParticle(new EParticle());
  }
}
