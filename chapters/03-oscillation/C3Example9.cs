using Godot;

public class C3Example9 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.9:\n" +
      "The Wave";
  }

  public float Radius = 30;
  public int Separation = 24;

  private float startAngle;
  private float angularVelocity = 0.1f;

  public override void _Draw()
  {
    var size = GetViewportRect().Size;
    float angle = startAngle;

    for (int x = 0; x <= size.x; x += Separation)
    {
      float y = Utils.Map(Mathf.Sin(angle), -1, 1, size.y / 4, size.y / 2 + size.y / 4);
      DrawCircle(new Vector2(x, y), Radius, Colors.LightBlue.WithAlpha(100));
      DrawCircle(new Vector2(x, y), Radius - 2, Colors.White.WithAlpha(100));
      angle += angularVelocity;
    }
  }

  public override void _Process(float delta)
  {
    startAngle += delta;
    Update();
  }
}
