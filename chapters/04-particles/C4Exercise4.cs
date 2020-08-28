using Godot;
using Drawing;
using Particles;

public class C4Exercise4 : C3Exercise5, IExample
{
  public new string _Summary()
  {
    return "Exercise 4.4:\n"
      + "Asteroids with Particles\n\n"
      + "On desktop, use left and right arrow keys to turn, then up arrow key to thrust.\n"
      + "On mobile, you can use the virtual controls.";
  }

  public class SpaceshipWithParticles : Spaceship
  {
    private SimpleParticleSystem particleSystem;

    public override void _Ready()
    {
      base._Ready();

      particleSystem = new SimpleParticleSystem();
      particleSystem.ParticlesContainer = GetParent();
      particleSystem.WrapMode = WrapModeEnum.None;
      particleSystem.LocalCoords = false;
      particleSystem.Emitting = false;
      particleSystem.Position = new Vector2(0, MeshSize.y + 10);
      particleSystem.ParticleSpawnFrameDelay = 0;
      particleSystem.ShowBehindParent = true;
      particleSystem.ParticleCreationFunction = () =>
      {
        var particle = new SimpleFallingParticle();
        particle.ForceRangeX = new Vector2(-0.15f, 0.15f);
        particle.ForceRangeY = new Vector2(-0.15f, 0.15f);
        particle.WrapMode = WrapModeEnum.None;
        particle.MeshSize = new Vector2(10, 10);
        particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
        particle.Lifespan = 2;
        return particle;
      };
      AddChild(particleSystem);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      particleSystem.Emitting = thrusting;
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
