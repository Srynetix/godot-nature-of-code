using Godot;

public class C4Exercise1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 4.1:\n"
      + "Particle With Forces";
  }

  public class EParticle : SimpleParticle
  {
    protected override void UpdateAcceleration()
    {
      ApplyForce(new Vector2((float)GD.RandRange(-0.25f, 0.25f), 0.25f));
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var particle = new EParticle();
    particle.Position = size / 2;
    particle.BodySize = new Vector2(10, 10);
    particle.Lifespan = 4;

    AddChild(particle);
  }
}
