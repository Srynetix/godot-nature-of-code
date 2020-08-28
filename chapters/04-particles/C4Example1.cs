using Godot;
using Particles;

public class C4Example1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.1:\n"
      + "Simple Particle";
  }

  public class EParticle : SimpleParticle
  {
    protected override void UpdateAcceleration()
    {
      Acceleration = new Vector2(0, 0.01f);
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particle = new EParticle();
    particle.Position = size / 2;
    AddChild(particle);
  }
}
