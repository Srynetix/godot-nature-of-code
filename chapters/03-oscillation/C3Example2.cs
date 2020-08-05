using System.Linq;

using Godot;

public class C3Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.2:\n"
      + "Forces with (arbitrary) angular motion";
  }

  public class Square : Mover
  {
    public override void _Ready()
    {
      bodySize = (float)GD.RandRange(5, 20);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(Vector2.Zero, Vector2.One * (bodySize + 1)), Colors.Black);
      DrawRect(new Rect2(Vector2.One, Vector2.One * (bodySize - 1)), Colors.LightBlue);
    }

    public override void _Process(float delta)
    {
      acceleration += new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1)) * delta;

      velocity += acceleration;
      velocity.x = Mathf.Clamp(velocity.x, -1.0f, 1.0f);
      velocity.y = Mathf.Clamp(velocity.y, -1.0f, 1.0f);

      Position += velocity;

      angularAcceleration = acceleration.x / 10.0f;
      angularVelocity += angularAcceleration;
      angularVelocity = Mathf.Clamp(angularVelocity, -0.1f, 0.1f);
      Rotation += angularVelocity;

      WrapEdges();
      Update();
    }
  }

  public override void _Draw()
  {
    DrawCircle(Vector2.Zero, 20, Colors.LightBlue);
  }

  public override void _Ready()
  {
    Position = GetViewport().Size / 2;

    // Spawn squares
    int squareCount = 20;
    foreach (int x in Enumerable.Range(0, squareCount))
    {
      var square = new Square();
      AddChild(square);
    }
  }
}
