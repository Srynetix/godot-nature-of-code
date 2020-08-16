using Godot;

public class SimpleParticle : SimpleMover
{
  public float Lifespan = 2;
  public bool LifespanAsAlpha = true;
  public Color BaseColor = Colors.White;
  public Color BaseOutlineColor = Colors.LightBlue;

  private float initialLifespan;

  public SimpleParticle(WrapModeEnum wrapMode = WrapModeEnum.None) : base(wrapMode) { }

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
    DrawCircle(Vector2.Zero, Radius, BaseOutlineColor.WithAlpha(alpha));
    DrawCircle(Vector2.Zero, Radius - 2, BaseColor.WithAlpha(alpha));
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