using Godot;

using System.Collections.Generic;

public class SimpleParticleSystem : SimpleMover
{
  public delegate SimpleParticle CreateParticleFunction();

  public bool Emitting = false;

  private List<SimpleParticle> particles;
  private CreateParticleFunction particleFunction = null;

  public SimpleParticleSystem(WrapModeEnum wrapMode = WrapModeEnum.Wrap) : base(wrapMode)
  {
    particles = new List<SimpleParticle>();
  }

  public void SetCreateParticleFunction(CreateParticleFunction fn)
  {
    particleFunction = fn;
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

    if (Emitting && particleFunction != null)
    {
      var particle = particleFunction();
      AddParticle(particle);
    }

    UpdateParticles();
  }

  public override void _Draw()
  {
    // No draw
  }
}