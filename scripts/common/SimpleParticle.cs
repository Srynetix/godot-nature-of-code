using Godot;

public class SimpleParticle : SimpleMover
{
  public float Lifespan = 2;
  public bool LifespanAsAlpha = true;

  private float initialLifespan;

  public override void _Ready()
  {
    initialLifespan = Lifespan;
  }

  public override void _Process(float delta)
  {
    if (IsDead())
    {
      return;
    }

    base._Process(delta);

    Lifespan -= delta;
  }

  public override void _Draw()
  {
    if (IsDead())
    {
      return;
    }

    var alpha = LifespanAsAlpha ? GetLifespanAlphaValue() : (byte)255;
    DrawCircle(Vector2.Zero, Radius, Colors.LightBlue.WithAlpha(alpha));
  }

  protected byte GetLifespanAlphaValue()
  {
    return (byte)((Lifespan / initialLifespan) * 255);
  }

  public bool IsDead()
  {
    return Lifespan <= 0;
  }
}