using Godot;

public class C4Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.4:\n"
      + "Multiple Particle Systems\n\n"
      + "Touch screen to spawn particle system";
  }

  public class EParticle : SimpleSquareParticle
  {
    protected override void UpdateAcceleration()
    {
      Acceleration = new Vector2((float)GD.RandRange(-0.5f, 0.5f), 0.15f);
      AngularAcceleration = Acceleration.x / 10f;
    }
  }

  public void AddParticleSystem(Vector2 position)
  {
    var ps = new SimpleParticleSystem();
    ps.Emitting = true;
    ps.SetCreateParticleFunction(CreateParticle);
    ps.GlobalPosition = position;
    AddChild(ps);
  }

  public SimpleParticle CreateParticle()
  {
    var particle = new EParticle();
    particle.Lifespan = 2;
    particle.BodySize = new Vector2(10, 10);
    return particle;
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        AddParticleSystem(eventScreenTouch.Position);
      }
    }
  }
}
