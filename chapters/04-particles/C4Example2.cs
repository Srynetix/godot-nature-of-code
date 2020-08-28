using Godot;
using System.Collections.Generic;
using Drawing;
using Particles;

public class C4Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.2:\n"
      + "Particles List";
  }

  public class ParticleList : Node2D
  {
    private List<SimpleParticle> particles;

    public ParticleList()
    {
      particles = new List<SimpleParticle>();
    }

    private void CreateParticle()
    {
      var particle = new SimpleFallingParticle();
      particle.MeshSize = new Vector2(20, 20);
      particle.Lifespan = 2;
      particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
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
      CreateParticle();
      UpdateParticles();
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var list = new ParticleList();
    list.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(list);
  }
}
