using Godot;

namespace Examples
{
  /// <summary>
  /// Example 1.1 - Bouncing ball, no vectors.
  /// </summary>
  /// Uses _Draw function and manual float coordinates (x/y) and speed.
  public class C1Example1 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 1.1:\n"
        + "Bouncing ball, no vectors";
    }

    private float x;
    private float y;
    private float xSpeed = 1f;
    private float ySpeed = 3.3f;

    public override void _Draw()
    {
      DrawCircle(new Vector2(x, y), 20, Colors.LightBlue);
      DrawCircle(new Vector2(x, y), 18, Colors.White);
    }

    public override void _Process(float delta)
    {
      var size = GetViewportRect().Size;

      x += xSpeed;
      y += ySpeed;

      if ((x > size.x) || (x < 0))
      {
        xSpeed *= -1;
      }

      if ((y > size.y) || (y < 0))
      {
        ySpeed *= -1;
      }

      Update();
    }
  }
}
