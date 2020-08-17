using Godot;

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
    particle.BodySize = new Vector2(20, 20);
    particle.Lifespan = 4;

    AddChild(particle);
  }
}
