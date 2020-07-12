using Godot;

public class C0Exercise8 : Node2D, IExample {
  public string _Summary() {
    return "Exercise I.8:\n"
      + "Play with color, noiseDetail(), and the rate at which xoff and yoff are incremented to achieve different visual effects.";
  }

  private Sprite sprite;
  private Image image;
  private ImageTexture texture;
  private OpenSimplexNoise noise;

  public override void _Ready() {
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

  private void GenerateNoiseTexture() {
    var size = GetViewport().Size;

    noise.Octaves = 8;

    image.Lock();
    for (int x = 0; x < size.x; ++x) {
      for (int y = 0; y < size.y; ++y) {
        float n = Utils.Map(noise.GetNoise2d(x + GD.Randf() * 10, y + GD.Randf() * 10), -1, 1, 0, 1);
        byte tint = (byte)Utils.Map(n, 0, 1, 0, 255);
        image.SetPixel(x, y, Color.Color8(tint, tint, tint));
      }
    }
    image.Unlock();
  }
}
