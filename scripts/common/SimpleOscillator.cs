using Godot;

public class SimpleOscillator : Node2D
{
  public bool ShowLine = true;

  public float Radius = 30;
  public Vector2 Angle;
  public Vector2 PositionOffset;
  public Vector2 Velocity;
  public Vector2 Amplitude;
  public Vector2 AngularAcceleration;

  public Color LineColor = Colors.LightGray;
  public Color BallColor = Colors.White;

  private SimpleCircleSprite circleSprite;
  private SimpleLineSprite lineSprite;

  public SimpleOscillator()
  {
    Velocity = new Vector2((float)GD.RandRange(-0.05f, 0.05f), (float)GD.RandRange(-0.05f, 0.05f));
    circleSprite = new SimpleCircleSprite();
    lineSprite = new SimpleLineSprite();
  }

  public override void _Ready()
  {
    circleSprite.Radius = Radius;
    circleSprite.BaseColor = BallColor;
    lineSprite.Drawing = ShowLine;
    lineSprite.BaseColor = LineColor;
    AddChild(lineSprite);
    AddChild(circleSprite);
  }

  public override void _Process(float delta)
  {
    Velocity += AngularAcceleration;
    Angle += Velocity;
    AngularAcceleration = Vector2.Zero;

    float x = PositionOffset.x + Mathf.Sin(Angle.x) * Amplitude.x;
    float y = PositionOffset.y + Mathf.Sin(Angle.y) * Amplitude.y;
    var target = new Vector2(x, y);

    lineSprite.LineA = GlobalPosition;
    lineSprite.LineB = GlobalPosition + target;
    circleSprite.Position = target;
  }
}
