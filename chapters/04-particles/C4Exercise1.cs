using Godot;
using Forces;
using Particles;

namespace Examples.Chapter4
{
  /// <summary>
  /// Exercise 4.1 - Particle With Forces.
  /// </summary>
  /// Use SimpleMover force capabilities at the core of SimpleParticle.
  public class C4Exercise1 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 4.1:\n"
        + "Particle With Forces";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var particle = new SimpleFallingParticle();
      particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
      particle.Position = size / 2;
      particle.MeshSize = new Vector2(20, 20);
      particle.Lifespan = 4;
      AddChild(particle);
    }
  }
}
