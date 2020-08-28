using Godot;

public class C0Example5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example I.5:\n"
      + "Perlin noise walker";
  }

  private OpenSimplexNoise noise;
  private Vector2 position;
  private float tx;
  private float ty;

  public override void _Ready()
  {
    noise = new OpenSimplexNoise();
    position = GetViewportRect().Size / 2;

    tx = 0;
    ty = 10000;
  }

  public override void _Draw()
  {
    float nx = noise.GetNoise1d(tx);
    float ny = noise.GetNoise1d(ty);
    float x = MathUtils.Map(nx, 0, 1, 0, GetViewportRect().Size.x / 4);
    float y = MathUtils.Map(ny, 0, 1, 0, GetViewportRect().Size.y / 4);

    var newPosition = position + new Vector2(x, y);

    DrawCircle(newPosition, 20, Colors.Black);
    DrawCircle(newPosition, 18, Colors.LightGray);
  }

  public override void _Process(float delta)
  {
    Update();

    tx += delta * 10;
    ty += delta * 10;
  }
}
