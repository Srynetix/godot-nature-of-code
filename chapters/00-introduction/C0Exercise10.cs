using Godot;

public class C0Exercise10 : Node2D, IExample {
  public string _Summary() {
    return "Exercise I.10:\n"
      + "Use the noise values as the elevations of a landscape. See the screenshot below as a reference.\n"
      + "Extra: This will be in 2D";
  }

  private OpenSimplexNoise noise;
  private float t;

  public override void _Ready() {
    noise = new OpenSimplexNoise();
    t = 0;
  }

  public override void _Draw() {
    var size = GetViewport().Size;
    var middleY = size.y / 2;
    float stepSize = size.x / 20.0f;
    float prevY = middleY;
    float nValue = 0;

    float tx = t;

    for (float x = 0; x < size.x; x += stepSize) {
      nValue = Utils.Map(noise.GetNoise1d(tx), -1, 1, -100, 100);
      DrawLine(new Vector2(x, prevY), new Vector2(x + stepSize, prevY + nValue), Colors.LightCyan, 2, true);

      tx += 100f;
      prevY += nValue;
    }
  }

  public override void _Process(float delta) {
    Update();
    t += 0.1f;
  }
}
