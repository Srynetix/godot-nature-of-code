using Godot;
using Forces;
using System.Collections.Generic;

namespace Particles
{
  /// <summary>
  /// Simple particle system.
  /// </summary>
  public class SimpleParticleSystem : SimpleMover
  {
    /// <summary>Particle creation function definition</summary>
    public delegate SimpleParticle ParticleCreationFunc();

    /// <summary>System is emitting</summary>
    public bool Emitting = true;

    /// <summary>Remove system when particle list is empty</summary>
    public bool RemoveWhenEmptyParticles = false;

    /// <summary>Particle count. Use -1 for "unlimited" particles</summary>
    public int ParticleCount = -1;

    /// <summary>Particle spawn frame delay</summary>
    public int ParticleSpawnFrameDelay = 4;

    /// <summary>Particle count per wave</summary>
    public int ParticleCountPerWave = 1;

    /// <summary>Draw in local coords</summary>
    public bool LocalCoords = true;

    /// <summary>Particles container. Use Parent by default.
    public Node ParticlesContainer = null;

    /// <summary>Particles creation function</summary>
    public ParticleCreationFunc ParticleCreationFunction = null;

    private List<SimpleParticle> particles;
    private int elapsedFrames = 0;

    /// <summary>
    /// Create a default particle system.
    /// </summary>
    public SimpleParticleSystem() : this(WrapModeEnum.None) { }

    /// <summary>
    /// Create a default particle system with a custom wrap mode.
    /// </summary>
    /// <param name="wrapMode">Wrap mode</param>
    public SimpleParticleSystem(WrapModeEnum wrapMode) : base(wrapMode)
    {
      particles = new List<SimpleParticle>();
      DisableForces = true;
      Drawing = false;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      if (Emitting && ParticleCreationFunction != null && ParticleCount != 0)
      {
        if (elapsedFrames == 0)
        {
          for (int i = 0; i < ParticleCountPerWave; ++i)
          {
            var particle = ParticleCreationFunction();
            AddParticle(particle);

            if (ParticleCount > 0)
            {
              ParticleCount--;
            }

            elapsedFrames = ParticleSpawnFrameDelay;
          }
        }
        else
        {
          elapsedFrames--;
        }
      }

      if (RemoveWhenEmptyParticles && particles.Count == 0)
      {
        QueueFree();
      }

      UpdateParticles();
    }

    private void AddParticle(SimpleParticle particle)
    {
      particles.Add(particle);

      if (LocalCoords)
      {
        AddChild(particle);
      }
      else
      {
        var container = ParticlesContainer ?? GetParent();
        particle.GlobalPosition = GlobalPosition + particle.InitialOffset;
        container.AddChild(particle);
      }
    }

    private void UpdateParticles()
    {
      List<SimpleParticle> newParticles = new List<SimpleParticle>();
      foreach (SimpleParticle part in particles)
      {
        if (part.IsDead())
        {
          part.QueueFree();
        }
        else
        {
          newParticles.Add(part);
        }
      }

      particles = newParticles;
    }
  }
}
