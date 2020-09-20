using Godot;

namespace Drawing
{
  /// <summary>
  /// Simple noise texture.
  /// </summary>
  public class SimpleNoiseTexture : Node2D
  {
    /// <summary>Noise factor</summary>
    public float Factor = 1;

    /// <summary>Noise octaves</summary>
    public int Octaves = 1;

    /// <summary>Sprite</summary>
    protected Sprite sprite;

    /// <summary>Image</summary>
    protected Image image;

    /// <summary>Texture</summary>
    protected ImageTexture texture;

    /// <summary>Noise</summary>
    protected OpenSimplexNoise noise;

    /// <summary>Image size</summary>
    protected Vector2 imageSize;

    /// <summary>
    /// Compute noise for X and Y coordinates.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns>Noise value</returns>
    protected virtual float ComputeNoise(float x, float y)
    {
      return noise.GetNoise2d(x, y);
    }

    /// <summary>
    /// Generate noise texture.
    /// </summary>
    protected void GenerateNoiseTexture()
    {
      image.Lock();
      for (int x = 0; x < imageSize.x; ++x)
      {
        for (int y = 0; y < imageSize.y; ++y)
        {
          float n = MathUtils.Map(ComputeNoise(x, y), -1, 1, 0, 1);
          byte tint = (byte)MathUtils.Map(n, 0, 1, 0, 255);
          image.SetPixel(x, y, Color.Color8(tint, tint, tint));
        }
      }
      image.Unlock();
    }

    public override void _Ready()
    {
      sprite = new Sprite();
      image = new Image();
      texture = new ImageTexture();
      noise = new OpenSimplexNoise { Octaves = Octaves };

      var viewportSize = GetViewportRect().Size;
      imageSize = viewportSize / Factor;
      imageSize = new Vector2((int)imageSize.x, (int)imageSize.y);
      image.Create((int)imageSize.x, (int)imageSize.y, false, Image.Format.Rgba8);

      // Generate
      GenerateNoiseTexture();

      // Create texture
      texture.CreateFromImage(image);

      // Prepare sprite
      sprite.Texture = texture;
      sprite.Position = viewportSize / 2;
      sprite.Scale = new Vector2(Factor, Factor);
      AddChild(sprite);
    }
  }
}
