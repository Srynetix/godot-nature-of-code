using System.Linq;

using Godot;

public class C3Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.2:\n"
      + "Forces with (arbitrary) angular motion";
  }

  public class Square : SimpleMover
  {
    public Square() : base(WrapModeEnum.Wrap)
    {
      MaxVelocity = 2.0f;
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(Vector2.Zero, Vector2.One * (BodySize + 1)), Colors.LightBlue);
      DrawRect(new Rect2(Vector2.One, Vector2.One * (BodySize - 1)), Colors.White);
    }

    protected override void UpdateAcceleration()
    {
      Acceleration += new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1)) * 0.02f;
      AngularAcceleration = Acceleration.x / 10.0f;
    }
  }

  public override void _Draw()
  {
    var size = GetViewport().Size;
    DrawCircle(size / 2, 20, Colors.LightBlue);
  }

  public override void _Ready()
  {
    var size = GetViewport().Size;

    // Spawn squares
    int squareCount = 20;
    foreach (int x in Enumerable.Range(0, squareCount))
    {
      var square = new Square();
      square.BodySize = (float)GD.RandRange(5, 20);
      square.Position = Utils.RandVector2(size / 8, size - size / 8);
      AddChild(square);
    }
  }
}
