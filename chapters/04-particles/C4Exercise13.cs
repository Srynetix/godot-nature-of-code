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
      particle.WrapMode = SimpleMover.WrapModeEnum.None;
      particle.ParticleMesh = ParticleMeshEnum.Texture;
      particle.ParticleTextureChoice = ParticleTexture.Choice.WhiteDotBlur;
      particle.BodySize = new Vector2(40, 40);
      particle.ForceRangeX = new Vector2(-0.25f, 0.25f);
      particle.BaseColor = Utils.RandColor();
      particle.ForceRangeY = new Vector2(0, -0.15f);
      particle.Lifespan = 2;
      return particle;
    });
    particleSystem.ParticleSpawnFrameDelay = 2;
    particleSystem.Position = new Vector2(size.x / 2, size.y / 1.5f);
    AddChild(particleSystem);
  }
}
