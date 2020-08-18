using Godot;

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
    particleSystem.SetCreateParticleFunction(() =>
    {
      var particle = new SimpleFallingParticle();
      particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
      particle.BodySize = new Vector2(20, 20);
      particle.Mesh.MeshType = SimpleMeshTypeEnum.Square;
      particle.Lifespan = 2;
      return particle;
    });
    particleSystem.ParticleSpawnFrameDelay = 2;
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    AddChild(particleSystem);
  }
}
