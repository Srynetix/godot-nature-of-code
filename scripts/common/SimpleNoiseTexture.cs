using Godot;

public class SimpleNoiseTexture : Node2D
{
  public float Factor = 1;
  public int Octaves = 1;

  protected Sprite sprite;
  protected Image image;
  protected ImageTexture texture;
  protected OpenSimplexNoise noise;

  public override void _Ready()
  {
    sprite = new Sprite();
    image = new Image();
    texture = new ImageTexture();
    noise = new OpenSimplexNoise();
    noise.Octaves = Octaves;

    var viewportSize = GetViewportRect().Size;
    var imageSize = viewportSize / Factor;
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

  protected virtual float ComputeNoise(float x, float y)
  {
    return noise.GetNoise2d(x, y);
  }

  protected void GenerateNoiseTexture()
  {
    var imageSize = GetViewportRect().Size / Factor;

    image.Lock();
    for (int x = 0; x < imageSize.x; ++x)
    {
      for (int y = 0; y < imageSize.y; ++y)
      {
        float n = Utils.Map(ComputeNoise(x, y), -1, 1, 0, 1);
        byte tint = (byte)Utils.Map(n, 0, 1, 0, 255);
        image.SetPixel(x, y, Color.Color8(tint, tint, tint));
      }
    }
    image.Unlock();
  }
}