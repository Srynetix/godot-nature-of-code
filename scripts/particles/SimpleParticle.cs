using Godot;

public class SimpleParticle : SimpleMover
{
  public float Lifespan = 2;
  public bool LifespanAsAlpha = true;

  private float initialLifespan;

  public SimpleParticle()
  {
    WrapMode = WrapModeEnum.None;
  }

  public override void _Ready()
  {
    base._Ready();
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

    if (LifespanAsAlpha)
    {
      var alpha = GetLifespanAlphaValue();
      Mesh.Modulate = Mesh.Modulate.WithAlpha(alpha);
    }
  }

  protected byte GetLifespanAlphaValue()
  {
    return (byte)Mathf.Clamp(((Lifespan / initialLifespan) * 255), 0, 255);
  }

  public bool IsDead()
  {
    return Lifespan <= 0;
  }
}
