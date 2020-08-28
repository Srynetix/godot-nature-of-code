using Godot;

public class C3Exercise6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.6:\n"
      + "Bob";
  }

  public class Ball : Node2D
  {
    public float Radius = 50;

    public float Amplitude = 300;
    public float Period = 75;

    private float frameCount;

    public override void _Draw()
    {
      var y = Amplitude * MathUtils.Map(Mathf.Cos(Mathf.Pi * 2 * frameCount / Period), -1, 1, 0.5f, 1);
      var target = new Vector2(0, y);

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
    var size = GetViewportRect().Size;
    var ball = new Ball();
    ball.Position = new Vector2(size.x / 2, 0);
    AddChild(ball);
  }
}
