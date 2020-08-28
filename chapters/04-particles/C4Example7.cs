using Godot;
using Drawing;
using Forces;
using Particles;

public class C4Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.7:\n"
      + "Particle Repeller";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particleSystem = new SimpleParticleSystem();
    particleSystem.Position = new Vector2(size.x / 2, size.y / 4);
    particleSystem.ParticleCreationFunction = () =>
    {
      var particle = new SimpleFallingParticle();
      particle.MeshSize = new Vector2(20, 20);
      particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
      particle.Lifespan = 2;
      particle.Mass = 2;
      return particle;
    };
    AddChild(particleSystem);

    var repeller = new SimpleRepeller();
    repeller.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(repeller);
  }
}
