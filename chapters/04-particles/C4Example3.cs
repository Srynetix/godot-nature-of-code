using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples
{
  namespace Chapter4
  {
    /// <summary>
    /// Example 4.3 - Particle System.
    /// </summary>
    /// Uses SimpleParticleSystem with its ParticleCreationFunction.
    public class C4Example3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 4.3:\n"
          + "Particle System";
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var particleSystem = new SimpleParticleSystem();
        particleSystem.ParticleCreationFunction = () =>
        {
          var particle = new SimpleFallingParticle();
          particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
          particle.MeshSize = new Vector2(20, 20);
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
          particle.Lifespan = 2;
          return particle;
        };
        particleSystem.ParticleSpawnFrameDelay = 2;
        particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
        AddChild(particleSystem);
      }
    }
  }
}
