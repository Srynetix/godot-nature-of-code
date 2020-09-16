using Godot;
using Assets;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.13 - Rainbow!
  /// </summary>
  /// Use SimpleMesh capabilities to spawn particles of multiple colors.
  public class C4Exercise13 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 4.13\n"
        + "Rainbow!";
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
            WrapMode = SimpleMover.WrapModeEnum.Bounce,
            MeshSize = new Vector2(40, 40),
            ForceRangeX = new Vector2(-0.75f, 0.75f),
            ForceRangeY = new Vector2(0, -0.25f),
            Lifespan = 2
          };
          particle.Mesh.MeshType = SimpleMesh.TypeEnum.Texture;
          particle.Mesh.CustomTexture = SimpleDefaultTexture.WhiteDotBlurTexture;
          particle.Mesh.CustomMaterial = SimpleDefaultMaterial.AddMaterial;
          particle.Mesh.Modulate = MathUtils.RandColor();
          return particle;
        },
        ParticleSpawnFrameDelay = 2,
        Position = new Vector2(size.x / 2, size.y / 1.5f)
      };
      AddChild(particleSystem);
    }
  }
}
