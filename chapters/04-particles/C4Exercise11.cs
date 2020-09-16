using Godot;
using Assets;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.11 - Fire!
  /// </summary>
  /// Use SimpleMesh capabilities to simulate a flame.
  public class C4Exercise11 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 4.11\n"
        + "Fire!";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var particleSystem = new SimpleParticleSystem
      {
        ParticleCreationFunction = () =>
        {
          var particle = new SimpleFallingParticle
          {
            WrapMode = SimpleMover.WrapModeEnum.None,
            MeshSize = new Vector2(150, 150),
            ForceRangeX = new Vector2(-0.25f, 0.25f),
            ForceRangeY = new Vector2(0, -0.15f),
            Lifespan = 2
          };
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Texture;
          particle.Mesh.CustomTexture = SimpleDefaultTexture.WhiteDotBlurTexture;
          particle.Mesh.CustomMaterial = SimpleDefaultMaterial.AddMaterial;
          particle.Mesh.Modulate = Colors.Firebrick;
          return particle;
        },
        ParticleSpawnFrameDelay = 2,
        Position = new Vector2(size.x / 2, size.y / 1.5f)
      };
      AddChild(particleSystem);
    }
  }
}
