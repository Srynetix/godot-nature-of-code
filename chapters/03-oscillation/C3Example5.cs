using Godot;

public class C3Example5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.5:\n"
      + "Simple Harmonic Motion";
  }

  public class Ball : Node2D
  {
    public float Radius = 50;

    // Amplitude == Rope length
    public float Amplitude = 250;
    // Period == Movement speed
    public float Period = 120;

    private float frameCount;

    public override void _Draw()
    {
      var x = Amplitude * Mathf.Cos(Mathf.Pi * 2 * frameCount / Period);
      var target = new Vector2(x, 0);

      DrawLine(Vector2.Zero, target, Colors.LightGray, 2);
      DrawCircle(target, Radius, Colors.LightBlue);
      DrawCircle(target, Radius - 2, Colors.White);
    }

    public override void _Process(float delta)
    {
      frameCount += 1;

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
