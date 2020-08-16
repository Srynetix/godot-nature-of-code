using Godot;

using System.Collections.Generic;

public class SimpleParticleSystem : SimpleMover
{
  private List<SimpleParticle> particles;

  public SimpleParticleSystem(WrapModeEnum wrapMode = WrapModeEnum.Wrap): base(wrapMode)
  {
    particles = new List<SimpleParticle>();
  }

  public void AddParticle(SimpleParticle particle)
  {
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
    base._Process(delta);

    UpdateParticles();
  }

  public override void _Draw() {
    // No draw
  }
}