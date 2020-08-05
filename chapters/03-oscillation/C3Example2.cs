using System.Linq;

using Godot;

public class C3Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.2:\n"
      + "Forces with (arbitrary) angular motion";
  }

  public class Square : Node2D
  {
    Vector2 velocity = Vector2.Zero;
    Vector2 acceleration = Vector2.One * 0.01f;
    float angularVelocity = 0;
    float angularAcceleration = 0.01f;
    float bodySize = 10;

    public override void _Ready()
    {
      bodySize = (float)GD.RandRange(5, 20);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(Vector2.Zero, Vector2.One * bodySize), Colors.LightBlue);
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

    private void WrapEdges()
    {
      var size = GetViewport().Size;

      if (GlobalPosition.x > size.x)
      {
        GlobalPosition = new Vector2(0, GlobalPosition.y);
      }
      else if (GlobalPosition.x < 0)
      {
        GlobalPosition = new Vector2(size.x, GlobalPosition.y);
      }

      if (GlobalPosition.y > size.y)
      {
        GlobalPosition = new Vector2(GlobalPosition.x, 0);
      }
      else if (GlobalPosition.y < 0)
      {
        GlobalPosition = new Vector2(GlobalPosition.x, size.y);
      }
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
    int squareCount = 10;
    foreach (int x in Enumerable.Range(0, squareCount))
    {
      var square = new Square();
      AddChild(square);
    }
  }
}
