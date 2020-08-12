using Godot;

public class C3Example4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.4:\n"
      + "Polar to Cartesian";
  }

  public class Ball : Node2D
  {
    public float RopeLength = 150;
    public float Radius = 50;

    private float theta = 0;

    public override void _Draw()
    {
      // Get X and Y coordinates from angle value
      float x = RopeLength * Mathf.Cos(theta);
      float y = RopeLength * Mathf.Sin(theta);

      var target = new Vector2(x, y);

      DrawCircle(Vector2.Zero, Radius / 10, Colors.LightGray);
      DrawLine(Vector2.Zero, target, Colors.LightGray, 2);
      DrawCircle(target, Radius, Colors.LightBlue);
      DrawCircle(target, Radius - 2, Colors.White);
    }

    public override void _Process(float delta)
    {
      theta += delta;
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
