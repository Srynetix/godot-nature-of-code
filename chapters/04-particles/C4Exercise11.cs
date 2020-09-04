using Godot;
using Assets;
using Drawing;
using Forces;
using Particles;

namespace Examples
{
  namespace Chapter4
  {
    /// <summary>
    /// Exercise 4.11 - Fire!
    /// </summary>
    /// Use SimpleMesh capabilities to simulate a flame.
    public class C4Exercise11 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 4.11\n"
          + "Fire!";
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var particleSystem = new SimpleParticleSystem();
        particleSystem.ParticleCreationFunction = () =>
        {
          var particle = new SimpleFallingParticle();
          particle.WrapMode = SimpleMover.WrapModeEnum.None;
          particle.MeshSize = new Vector2(150, 150);
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Texture;
          particle.Mesh.CustomTexture = SimpleDefaultTexture.FromEnum(SimpleDefaultTexture.Enum.WhiteDotBlur);
          particle.Mesh.CustomMaterial = SimpleDefaultMaterial.FromEnum(SimpleDefaultMaterial.Enum.Add);
          particle.Mesh.Modulate = Colors.Firebrick;
          particle.ForceRangeX = new Vector2(-0.25f, 0.25f);
          particle.ForceRangeY = new Vector2(0, -0.15f);
          particle.Lifespan = 2;
          return particle;
        };
        particleSystem.ParticleSpawnFrameDelay = 2;
        particleSystem.Position = new Vector2(size.x / 2, size.y / 1.5f);
        AddChild(particleSystem);
      }
    }
  }
}
