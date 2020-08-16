using Godot;

public class C4Example5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.5:\n"
      + "Multiple Particle Types";
  }

  public class ERoundParticle : SimpleParticle
  {
    protected override void UpdateAcceleration()
    {
      ApplyForce(new Vector2(Utils.RandRangef(-0.5f, 0.5f), 0.0981f));
    }
  }

  public class ESquareParticle : SimpleSquareParticle
  {
    protected override void UpdateAcceleration()
    {
      ApplyForce(new Vector2(Utils.RandRangef(-0.5f, 0.5f), -0.0981f));
    }
  }

  public class EParticleSystem : SimpleParticleSystem
  {
    public override void _Ready()
    {
      base._Ready();

      SetCreateParticleFunction(CreateParticle);
      Emitting = true;
    }

    public SimpleParticle CreateParticle()
    {
      SimpleParticle particle = null;
      if (Utils.SignedRandf() > 0.5)
      {
        particle = new ERoundParticle();
        particle.BodySize = new Vector2(7.5f, 7.5f);
      }
      else
      {
        particle = new ESquareParticle();
        particle.BodySize = new Vector2(20, 20);
      }

      particle.Lifespan = 2;
      return particle;
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new EParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(particleSystem);
  }
}
