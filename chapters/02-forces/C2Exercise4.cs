using System.Linq;

using Godot;

public class C2Exercise4 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.4:\n"
      + "Friction Pockets";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.1f, 0);
      var gravity = new Vector2(0, 0.98f);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var zone1 = new SimpleFrictionPocket();
    zone1.Coeff = 0.25f;
    zone1.Size = new Vector2(100, size.y);
    zone1.Position = new Vector2(size.x / 4, size.y / 2);
    AddChild(zone1);

    var zone2 = new SimpleFrictionPocket();
    zone2.Coeff = -0.25f;
    zone2.Size = new Vector2(100, size.y);
    zone2.Position = new Vector2(size.x / 2 + size.x / 4, size.y / 2);
    AddChild(zone2);

    var zone3 = new SimpleFrictionPocket();
    zone3.Coeff = -2f;
    zone3.Size = new Vector2(10, size.y);
    zone3.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(zone3);

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      var bodySize = (float)GD.RandRange(5, 20);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
      mover.BodySize = bodySize;
      mover.Mass = (float)GD.RandRange(5, 10);
      mover.Position = new Vector2(xPos, size.y / 2);
      AddChild(mover);
    }
  }
}
