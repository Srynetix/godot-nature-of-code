using Godot;
using Drawing;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.4 - Asteroids with Particles.
  /// </summary>
  /// Reuses Asteroid from Exercise 3.5 with a SimpleParticleSystem.
  public class C4Exercise4 : Chapter3.C3Exercise5, IExample
  {
    public new string GetSummary()
    {
      return "Exercise 4.4:\n"
        + "Asteroids with Particles\n\n"
        + "On desktop, use left and right arrow keys to turn, then up arrow key to thrust.\n"
        + "On mobile, you can use the virtual controls.";
    }

    private class SpaceshipWithParticles : Spaceship
    {
      private SimpleParticleSystem particleSystem;

      public override void _Ready()
      {
        base._Ready();

        particleSystem = new SimpleParticleSystem
        {
          ParticlesContainer = GetParent(),
          WrapMode = WrapModeEnum.None,
          LocalCoords = false,
          Emitting = false,
          Position = new Vector2(0, MeshSize.y + 10),
          ParticleSpawnFrameDelay = 0,
          ShowBehindParent = true,
          ParticleCreationFunction = () =>
          {
            var particle = new SimpleFallingParticle
            {
              ForceRangeX = new Vector2(-0.15f, 0.15f),
              ForceRangeY = new Vector2(-0.15f, 0.15f),
              WrapMode = WrapModeEnum.None,
              MeshSize = new Vector2(10, 10),
              Lifespan = 2
            };
            particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
            return particle;
          }
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

      spaceship = new SpaceshipWithParticles
      {
        Position = GetViewportRect().Size / 2
      };
      AddChild(spaceship);
    }
  }
}
