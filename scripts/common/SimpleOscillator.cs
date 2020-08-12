using Godot;

public class SimpleOscillator : Node2D
{
  public float Radius = 30;

  public Vector2 Angle;
  public Vector2 Offset;
  public Vector2 Velocity;
  public Vector2 Amplitude;
  public Vector2 AngularAcceleration;

  public SimpleOscillator()
  {
    Velocity = new Vector2((float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f));
  }

  public override void _Draw()
  {
    float x = Offset.x + Mathf.Sin(Angle.x) * Amplitude.x;
    float y = Offset.y + Mathf.Sin(Angle.y) * Amplitude.y;
    var target = new Vector2(x, y);

    DrawLine(Vector2.Zero, target, Colors.LightGray, 2);
    DrawCircle(target, Radius, Colors.LightBlue);
    DrawCircle(target, Radius - 2, Colors.White);
  }

  public override void _Process(float delta)
  {
    Velocity += AngularAcceleration;
    Angle += Velocity;
    AngularAcceleration = Vector2.Zero;

    Update();
  }
}
