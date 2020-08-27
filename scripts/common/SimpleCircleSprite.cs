using Godot;

public class SimpleCircleSprite : Sprite
{
  public float Radius
  {
    get => radius;
    set
    {
      radius = value;

      if (Texture != null)
      {
        var targetSize = new Vector2(value * 1.5f, value * 1.5f);
        Scale = targetSize / Texture.GetSize();
      }
    }
  }
  public Color BaseColor = Colors.White;
  public bool OutlineOnly = false;

  private float radius = 10;

  public override void _Ready()
  {
    if (OutlineOnly)
    {
      Texture = SimpleDefaultTexture.FromEnum(SimpleDefaultTextureEnum.WhiteDotOutlineOnly);
    }
    else
    {
      Texture = SimpleDefaultTexture.FromEnum(SimpleDefaultTextureEnum.WhiteDotAlphaWithOutline);
    }

    var targetSize = new Vector2(Radius * 1.5f, Radius * 1.5f);
    Scale = targetSize / Texture.GetSize();
    SelfModulate = BaseColor;
  }
}
