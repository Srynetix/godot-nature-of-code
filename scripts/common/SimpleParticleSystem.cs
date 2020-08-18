using Godot;

using System.Collections.Generic;

public class SimpleParticleSystem : SimpleMover
{
  public delegate SimpleParticle CreateParticleFunction();

  public bool Emitting = true;
  public bool RemoveWhenEmptyParticles = false;
  public int ParticleCount = -1;
  public int ParticleSpawnFrameDelay = 4;
  public bool LocalCoords = true;
  public Node ParticlesContainer = null;

  private List<SimpleParticle> particles;
  private CreateParticleFunction particleFunction = null;
  private int elapsedFrames = 0;

  public SimpleParticleSystem(WrapModeEnum wrapMode = WrapModeEnum.Wrap) : base(wrapMode)
  {
    particles = new List<SimpleParticle>();
    DisableForces = true;
    Drawing = false;
  }

  public void SetCreateParticleFunction(CreateParticleFunction fn)
  {
    particleFunction = fn;
  }

  public void AddParticle(SimpleParticle particle)
  {
    particles.Add(particle);

    if (LocalCoords)
    {
      AddChild(particle);
    }
    else
    {
      var container = ParticlesContainer ?? GetParent();
      particle.GlobalPosition = GlobalPosition;
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

  public override void _Process(float delta)
  {
    base._Process(delta);

    if (Emitting && particleFunction != null && ParticleCount != 0)
    {
      if (elapsedFrames == 0)
      {
        var particle = particleFunction();
        AddParticle(particle);

        if (ParticleCount > 0)
        {
          ParticleCount--;
        }

        elapsedFrames = ParticleSpawnFrameDelay;
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
}