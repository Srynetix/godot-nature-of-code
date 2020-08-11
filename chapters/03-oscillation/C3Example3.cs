using Godot;

public class C3Example3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.3:\n"
      + "Pointing towards motion";
  }

  public class Mover : SimpleMover
  {
    protected override void UpdateAcceleration()
    {
      var mousePos = GetViewport().GetMousePosition();
      var dir = (mousePos - Position).Normalized();

      Acceleration = dir * 0.5f;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      // Update angle from velocity
      float angle = Mathf.Atan2(Velocity.y, Velocity.x);
      Rotation = angle;
    }

    public override void _Draw()
    {
      var length = BodySize * 2;
      var width = BodySize;

      DrawRect(new Rect2(-length / 2, -width / 2, length, width), Colors.LightBlue);
      DrawRect(new Rect2(-length / 2 + 2, -width / 2 + 2, length - 4, width - 4), Colors.White);
    }
  }

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewportRect().Size;

    var mover = new Mover();
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
    AddChild(mover);
  }
}
