using Godot;

public class C4Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 4.4:\n"
      + "Multiple Particle Systems\n\n"
      + "Touch screen to spawn particle system";
  }

  private void AddParticleSystem(Vector2 position)
  {
    var ps = new SimpleParticleSystem();
    ps.SetCreateParticleFunction(() =>
    {
      var particle = new SimpleFallingParticle();
      particle.Lifespan = 2;
      particle.Mesh.MeshType = SimpleMeshTypeEnum.Square;
      particle.BodySize = new Vector2(10, 10);
      return particle;
    });
    ps.GlobalPosition = position;
    AddChild(ps);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        AddParticleSystem(eventScreenTouch.Position);
      }
    }
  }

  public override void _Ready()
  {
    // Initial systems
    var size = GetViewportRect().Size;
    int initialCount = 2;
    float splitSize = size.x / initialCount;

    for (int i = 0; i < initialCount; ++i)
    {
      AddParticleSystem(new Vector2(splitSize / 2 + splitSize * i, size.y / 4));
    }
  }
}
