using Godot;
using Drawing;
using Forces;
using Particles;

public class C4Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.2:\n"
      + "Particle with Angular Velocity";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particle = new SimpleFallingParticle();
    particle.WrapMode = SimpleMover.WrapModeEnum.Bounce;
    particle.Position = size / 2;
    particle.MeshSize = new Vector2(20, 20);
    particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
    particle.Lifespan = 4;
    AddChild(particle);
  }
}
