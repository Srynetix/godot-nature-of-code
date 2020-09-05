using Godot;

namespace Examples.Chapter3
{
  /// <summary>
  /// Example 3.6 - Simple Harmonic Motion II.
  /// </summary>
  /// Same principle than Example 3.5 but without frame count.
  public class C3Example6 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 3.6:\n"
        + "Simple Harmonic Motion II";
    }

    private class Ball : Node2D
    {
      public float Radius = 50;
      public float Amplitude = 250;

      private float angle = 0;
      public float angularVelocity = 0.05f;

      public override void _Draw()
      {
        var x = Amplitude * Mathf.Cos(angle);
        var target = new Vector2(x, 0);

        angle += angularVelocity;

        DrawLine(Vector2.Zero, target, Colors.LightGray, 2);
        DrawCircle(target, Radius, Colors.LightBlue);
        DrawCircle(target, Radius - 2, Colors.White);
      }

      public override void _Process(float delta)
      {
        Update();
      }
    }

    public override void _Ready()
    {
      var ball = new Ball();
      ball.Position = GetViewportRect().Size / 2;
      AddChild(ball);
    }
  }
}
