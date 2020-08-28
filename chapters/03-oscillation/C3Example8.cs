using Godot;

public class C3Example8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.8:\n"
      + "Static line wave";
  }

  private float angle = 0;
  private float angularVelocity = 0.2f;

  public override void _Draw()
  {
    var size = GetViewportRect().Size;
    float prevY = 0;
    float prevX = 0;

    for (int x = 0; x <= size.x; x += 5)
    {
      float y = MathUtils.Map(Mathf.Sin(angle), -1, 1, size.y / 4, size.y / 2 + size.y / 4);

      // Ignore first point
      if (x != 0)
      {
        DrawLine(new Vector2(prevX, prevY), new Vector2(x, y), Colors.LightBlue, 2);
        angle += angularVelocity;
      }

      prevX = x;
      prevY = y;
    }
  }
}
