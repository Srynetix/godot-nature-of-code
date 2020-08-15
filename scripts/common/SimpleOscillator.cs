using Godot;

public class SimpleOscillator : Node2D
{
  public float Radius = 30;
  public bool ShowLine = true;

  public Vector2 Angle;
  public Vector2 Offset;
  public Vector2 Velocity;
  public Vector2 Amplitude;
  public Vector2 AngularAcceleration;

  public Color LineColor = Colors.LightGray;
  public Color BallOutlineColor = Colors.LightBlue;
  public Color BallColor = Colors.White;

  public SimpleOscillator()
  {
    Velocity = new Vector2((float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f));
  }

  public override void _Draw()
  {
    float x = Offset.x + Mathf.Sin(Angle.x) * Amplitude.x;
    float y = Offset.y + Mathf.Sin(Angle.y) * Amplitude.y;
    var target = new Vector2(x, y);

    if (ShowLine)
    {
      DrawLine(Vector2.Zero, target, LineColor, 2);
    }

    DrawCircle(target, Radius, BallOutlineColor);
    DrawCircle(target, Radius - 2, BallColor);
  }

  public override void _Process(float delta)
  {
    Velocity += AngularAcceleration;
    Angle += Velocity;
    AngularAcceleration = Vector2.Zero;

    Update();
  }
}
