using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.10 - Responding Particles.
  /// </summary>
  /// Each particle attract others.
  public class C4Exercise10 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 4.10:\n"
        + "Responding Particles";
    }

    public class EParticle : SimpleFallingParticle
    {
      public override void _Ready()
      {
        base._Ready();
        WrapMode = WrapModeEnum.Bounce;
        ForceRangeX = new Vector2(-0.1f, 0.1f);
        ForceRangeY = new Vector2(0.005f, 0.005f);

        // Every particle should attract each other
        var attractor = new SimpleAttractor {
          Visible = false
        };
        AddChild(attractor);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var particleSystem = new SimpleParticleSystem {
        ParticleCreationFunction = () =>
        {
          var particle = new EParticle {
            MeshSize = new Vector2(20, 20),
            Lifespan = 4,
            Mass = 4
          };
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
          return particle;
        },
        Position = new Vector2(size.x / 2, size.y / 4)
      };
      AddChild(particleSystem);
    }
  }
}
