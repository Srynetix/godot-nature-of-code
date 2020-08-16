using Godot;

using System.Collections.Generic;

public class C4Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.2:\n"
      + "Particles List";
  }

  public class EParticle : SimpleParticle
  {
    public EParticle() : base(WrapModeEnum.None) { }

    protected override void UpdateAcceleration()
    {
      AngularAcceleration = Acceleration.x / 10f;
      ApplyForce(new Vector2((float)GD.RandRange(-0.5f, 0.5f), 0.15f));
    }

    public override void _Draw()
    {
      if (IsDead())
      {
        return;
      }

      DrawRect(new Rect2(-BodySize / 2, BodySize / 2), Colors.LightBlue.WithAlpha(GetLifespanAlphaValue()));
    }
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
      var particle = new EParticle();
      particle.BodySize = new Vector2(20, 20);
      particle.Lifespan = 2;
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
