using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.3 - Dynamic Particle System.
  /// </summary>
  /// Uses SimpleMover capabilities at the core of SimpleParticleSystem.
  public class C4Exercise3 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 4.3:\n"
        + "Dynamic Particle System";
    }

    private class DynamicParticleSystem : SimpleParticleSystem
    {
      public DynamicParticleSystem() : base(WrapModeEnum.Bounce)
      {
        LocalCoords = false;
        DisableForces = false;
      }

      protected override void UpdateAcceleration()
      {
        const float offset = 0.5f;
        ApplyForce(new Vector2((float)GD.RandRange(-offset, offset), (float)GD.RandRange(-offset, offset)));
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var particleSystem = new DynamicParticleSystem
      {
        Position = new Vector2(size.x / 2, size.y / 4),
        ParticleSpawnFrameDelay = 1,
        ParticleCreationFunction = () =>
        {
          var particle = new SimpleFallingParticle
          {
            WrapMode = SimpleMover.WrapModeEnum.Bounce,
            MeshSize = new Vector2(20, 20),
            Lifespan = 2
          };
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
          return particle;
        }
      };
      AddChild(particleSystem);
    }
  }
}
