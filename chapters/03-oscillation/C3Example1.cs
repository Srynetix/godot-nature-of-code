using Godot;

public class C3Example1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.1:\n"
      + "Angular motion using rotate()";
  }

  float angularVelocity = 0;
  float angularAcceleration = 0.01f;
  float lineSize = 50;
  float ballRadius = 10;

  public override void _Draw()
  {
    DrawLine(Vector2.Left * lineSize, Vector2.Right * lineSize, Colors.White, 1, true);
    DrawCircle(Vector2.Left * lineSize, ballRadius, Colors.LightBlue);
    DrawCircle(Vector2.Right * lineSize, ballRadius, Colors.LightBlue);
  }

  public override void _Ready()
  {
    Position = GetViewport().Size / 2;
  }

  public override void _Process(float delta)
  {
    angularVelocity += angularAcceleration * delta;
    Rotation += angularVelocity;

    Update();
  }
}
