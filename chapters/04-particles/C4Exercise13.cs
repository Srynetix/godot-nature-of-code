using Godot;

public class C4Exercise13 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.13\n"
      + "Rainbow!";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.SetCreateParticleFunction(() =>
    {
      var particle = new SimpleFallingParticle();
      particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
      particle.MeshSize = new Vector2(40, 40);
      particle.Mesh.MeshType = SimpleMeshTypeEnum.Texture;
      particle.Mesh.CustomTexture = SimpleDefaultTexture.FromEnum(SimpleDefaultTexture.Enum.WhiteDotBlur);
      particle.Mesh.CustomTextureBlendMode = CanvasItemMaterial.BlendModeEnum.Add;
      particle.Mesh.Modulate = MathUtils.RandColor();
      particle.ForceRangeX = new Vector2(-0.75f, 0.75f);
      particle.ForceRangeY = new Vector2(0, -0.25f);
      particle.Lifespan = 2;
      return particle;
    });
    particleSystem.ParticleSpawnFrameDelay = 2;
    particleSystem.Position = new Vector2(size.x / 2, size.y / 1.5f);
    AddChild(particleSystem);
  }
}
