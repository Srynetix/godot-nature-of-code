using Godot;

/// <summary>
/// Use this to draw a circle.
/// Instead of DrawCircle, it can be batched.
/// </summary>
public class SimpleCircleSprite : Sprite
{
  /// <summary>
  /// Circle radius.
  /// </summary>
  public float Radius
  {
    get => radius;
    set
    {
      radius = value;
      UpdateScale();
    }
  }

  private float radius = 10;

  /// <summary>
  /// Create a light blue circle with radius 10.
  /// </summary>
  public SimpleCircleSprite()
  {
    Radius = 10;
    Modulate = Colors.LightBlue;
  }

  public override void _Ready()
  {
    Texture = SimpleDefaultTexture.FromEnum(SimpleDefaultTexture.Enum.WhiteDotAlphaWithOutline);
    UpdateScale();
  }

  private void UpdateScale()
  {
    if (Texture != null)
    {
      var targetSize = new Vector2(Radius * 2f, Radius * 2f);
      Scale = targetSize / Texture.GetSize();
    }
  }
}
