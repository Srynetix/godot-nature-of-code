using System.Linq;

using Godot;

public class C2Exercise5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.5:\n"
      + "The faster an object moves, the greater the drag force against it.\n"
      + "In fact, an object not moving in water experiences no drag at all.\n"
      + "Expand the example to drop the balls from different heights.\n"
      + "How does this affect the drag as they hit the water?";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.1f, 0);
      var gravity = new Vector2(0, 0.098f * Mass);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    var size = GetViewport().Size;

    var zone = new SimpleLiquid();
    zone.Coeff = 0.25f;
    zone.Size = new Vector2(size.x, size.y / 4);
    zone.Position = new Vector2(size.x / 2, size.y - size.y / 8);
    AddChild(zone);

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      var bodySize = (float)GD.RandRange(5, 20);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
      mover.BodySize = bodySize;
      mover.Mass = (float)GD.RandRange(5, 10);
      mover.Position = new Vector2(xPos, size.y / 2 + (float)GD.RandRange(-100, 100));
      AddChild(mover);
    }
  }
}
