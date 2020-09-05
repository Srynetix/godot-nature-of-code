using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.9 - Particle Repellers.
  /// </summary>
  /// Same principle as Example 4.7 but with multiple repellers.
  public class C4Exercise9 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 4.9:\n"
        + "Particle Repellers";
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
      repeller.Position = particleSystem.Position + new Vector2(0, 100);
      AddChild(repeller);

      var repeller2 = new SimpleRepeller();
      repeller2.Position = particleSystem.Position + new Vector2(-100, 0);
      AddChild(repeller2);
    }
  }
}
