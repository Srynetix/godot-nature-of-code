using Godot;

public class C4Example5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.5:\n"
      + "Multiple Particle Types";
  }

  public class ERoundParticle : SimpleFallingParticle
  {
    public ERoundParticle()
    {
      WrapMode = WrapModeEnum.Bounce;
      ForceRangeY = new Vector2(-0.15f, -0.15f);
      BodySize = new Vector2(7.5f, 7.5f);
      IsSquare = false;
    }
  }

  public class ESquareParticle : SimpleFallingParticle
  {
    public ESquareParticle()
    {
      WrapMode = WrapModeEnum.Bounce;
      ForceRangeY = new Vector2(0.15f, 0.15f);
      BodySize = new Vector2(20f, 20f);
      IsSquare = true;
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.SetCreateParticleFunction(() =>
    {
      SimpleParticle particle = null;

      if (Utils.RandRangef(0, 1) >= 0.5)
      {
        particle = new ERoundParticle();
      }
      else
      {
        particle = new ESquareParticle();
      }

      particle.Lifespan = 2;
      return particle;
    });
    particleSystem.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(particleSystem);
  }
}
