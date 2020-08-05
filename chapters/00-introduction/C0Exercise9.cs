using Godot;

public class C0Exercise9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.9:\n"
      + "Add a third argument to noise that increments once per cycle through draw() to animate the two-dimensional noise.";
  }

  private Sprite sprite;
  private Image image;
  private ImageTexture texture;
  private OpenSimplexNoise noise;
  private float time;

  public override void _Ready()
  {
    sprite = new Sprite();
    image = new Image();
    texture = new ImageTexture();
    noise = new OpenSimplexNoise();

    var size = GetViewport().Size;
    image.Create((int)size.x, (int)size.y, false, Image.Format.Rgba8);

    // Generate
    GenerateNoiseTexture();

    // Create texture
    texture.CreateFromImage(image);

    // Prepare sprite
    sprite.Texture = texture;
    sprite.Position = size / 2;
    AddChild(sprite);
  }

  public override void _Process(float delta)
  {
    GenerateNoiseTexture();
    texture.SetData(image);

    time += delta * 10;
  }

  private void GenerateNoiseTexture()
  {
    var size = GetViewport().Size;

    noise.Octaves = 8;

    image.Lock();
    for (int x = 0; x < size.x; ++x)
    {
      for (int y = 0; y < size.y; ++y)
      {
        float n = Utils.Map(noise.GetNoise3d(x, y, time), -1, 1, 0, 1);
        byte tint = (byte)Utils.Map(n, 0, 1, 0, 255);
        image.SetPixel(x, y, Color.Color8(tint, tint, tint));
      }
    }
    image.Unlock();
  }
}
