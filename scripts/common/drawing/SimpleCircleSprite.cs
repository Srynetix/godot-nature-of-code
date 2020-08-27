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

  public bool Drawing
  {
    get => drawing;
    set
    {
      drawing = value;
      UpdateDrawing();
    }
  }

  public Color BaseColor
  {
    get => baseColor;
    set
    {
      baseColor = value;
      UpdateDrawing();
    }
  }

  private Color baseColor = Colors.White;
  private bool drawing = true;
  private float radius = 10;

  public override void _Ready()
  {
    Texture = SimpleDefaultTexture.FromEnum(SimpleDefaultTextureEnum.WhiteDotAlphaWithOutline);

    var targetSize = new Vector2(Radius * 2f, Radius * 2f);
    Scale = targetSize / Texture.GetSize();
    UpdateDrawing();
  }

  private void UpdateDrawing()
  {
    if (drawing)
    {
      Modulate = baseColor;
    }
    else
    {
      Modulate = Colors.Transparent;
    }
  }
}
