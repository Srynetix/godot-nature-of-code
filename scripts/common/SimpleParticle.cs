using Godot;

public enum ParticleMeshEnum
{
  Round,
  Square,
  Texture
}

public class ParticleTexture
{
  public enum Choice
  {
    WhiteDot,
    WhiteDotBlur
  }

  public static Texture WhiteDotTexture;
  public static Texture WhiteDotBlurTexture;

  public static void Initialize()
  {
    if (WhiteDotTexture == null)
    {
      WhiteDotTexture = (Texture)GD.Load("res://assets/textures/white-dot-on-black.png");
    }

    if (WhiteDotBlurTexture == null)
    {
      WhiteDotBlurTexture = (Texture)GD.Load("res://assets/textures/white-dot-blur.png");
    }
  }
}

public class SimpleParticle : SimpleMover
{
  public float Lifespan = 2;
  public bool LifespanAsAlpha = true;
  public Color BaseColor = Colors.White;
  public Color BaseOutlineColor = Colors.LightBlue;
  public ParticleMeshEnum ParticleMesh = ParticleMeshEnum.Round;
  public ParticleTexture.Choice ParticleTextureChoice = ParticleTexture.Choice.WhiteDot;

  private Sprite sprite;
  private CanvasItemMaterial material;
  private float initialLifespan;

  public SimpleParticle(WrapModeEnum wrapMode = WrapModeEnum.None) : base(wrapMode)
  {
    // Lazy initialization
    ParticleTexture.Initialize();
  }

  public override void _Ready()
  {
    base._Ready();
    material = new CanvasItemMaterial();
    material.BlendMode = CanvasItemMaterial.BlendModeEnum.Add;
    sprite = new Sprite();
    sprite.Visible = false;
    sprite.Material = material;
    initialLifespan = Lifespan;
    AddChild(sprite);

    // Handle particle mesh
    if (ParticleMesh == ParticleMeshEnum.Texture)
    {
      if (ParticleTextureChoice == ParticleTexture.Choice.WhiteDot)
      {
        sprite.Texture = ParticleTexture.WhiteDotTexture;
      }
      else
      {
        sprite.Texture = ParticleTexture.WhiteDotBlurTexture;
      }
      sprite.Scale = BodySize / sprite.Texture.GetSize();
      sprite.Visible = true;
    }
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
      sprite.Modulate = sprite.Modulate.WithAlpha(GetLifespanAlphaValue());
    }
  }

  public override void _Draw()
  {
    if (IsDead())
    {
      return;
    }

    if (ParticleMesh == ParticleMeshEnum.Texture)
    {
      // Handled by sprite
      return;
    }

    var alpha = LifespanAsAlpha ? GetLifespanAlphaValue() : (byte)255;
    if (ParticleMesh == ParticleMeshEnum.Square)
    {
      DrawRect(new Rect2(-BodySize / 2, BodySize / 2), Colors.LightBlue.WithAlpha(GetLifespanAlphaValue()));
    }
    else
    {
      DrawCircle(Vector2.Zero, Radius, BaseOutlineColor.WithAlpha(alpha));
      DrawCircle(Vector2.Zero, Radius - 2, BaseColor.WithAlpha(alpha));
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
