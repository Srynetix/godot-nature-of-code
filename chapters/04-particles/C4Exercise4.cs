using Godot;

public class C4Exercise4 : C3Exercise5, IExample
{
  public new string _Summary()
  {
    return "Exercise 4.4:\n"
      + "Asteroids with Particles";
  }

  public class SpaceshipWithParticles : Spaceship
  {
    private SimpleParticleSystem particleSystem;

    public override void _Ready()
    {
      base._Ready();

      particleSystem = new SimpleParticleSystem();
      particleSystem.WrapMode = WrapModeEnum.None;
      particleSystem.Emitting = false;
      particleSystem.Position = new Vector2(0, Radius);
      particleSystem.ShowBehindParent = true;
      particleSystem.SetCreateParticleFunction(CreateParticle);
      AddChild(particleSystem);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      particleSystem.Emitting = thrusting;
    }

    private SimpleParticle CreateParticle()
    {
      var particle = new SimpleFallingParticle();
      particle.BodySize = new Vector2(10, 10);
      particle.Lifespan = 2;
      return particle;
    }
  }

  public override void _Ready()
  {
    // Add virtual controls
    controls = new VirtualControls();
    AddChild(controls);

    spaceship = new SpaceshipWithParticles();
    spaceship.Position = GetViewportRect().Size / 2;
    AddChild(spaceship);
  }
}
