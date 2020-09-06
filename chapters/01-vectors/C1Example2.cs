using Godot;

namespace Examples.Chapter1
{
  /// <summary>
  /// Example 1.2 - Bouncing call, vectors.
  /// </summary>
  /// Uses _Draw function and manual vector coordinates and speed.
  public class C1Example2 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 1.2:\n"
        + "Bouncing ball, vectors";
    }

    private Vector2 position = new Vector2(100, 100);
    private Vector2 velocity = new Vector2(2.5f, 5f);

    public override void _Draw()
    {
      DrawCircle(position, 20, Colors.LightBlue);
      DrawCircle(position, 18, Colors.White);
    }

    public override void _Process(float delta)
    {
      var size = GetViewportRect().Size;

      position += velocity;

      if ((position.x > size.x) || (position.x < 0))
      {
        velocity.x *= -1;
      }

      if ((position.y > size.y) || (position.y < 0))
      {
        velocity.y *= -1;
      }

      Update();
    }
  }
}
