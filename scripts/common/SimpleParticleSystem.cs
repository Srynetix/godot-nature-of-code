using Godot;

using System.Collections.Generic;

public class SimpleParticleSystem : Node2D
{
  private List<SimpleParticle> particles;

  public SimpleParticleSystem()
  {
    particles = new List<SimpleParticle>();
  }

  protected virtual void InitializeParticle(SimpleParticle particle)
  {
    particle.BodySize = new Vector2(20, 20);
    particle.Lifespan = 2;
  }

  public void AddParticle(SimpleParticle particle)
  {
    InitializeParticle(particle);
    particles.Add(particle);
    AddChild(particle);
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
    UpdateParticles();
  }
}