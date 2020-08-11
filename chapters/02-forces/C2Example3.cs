using System.Linq;

using Godot;

public class C2Example3 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.3:\n"
      + "Gravity scaled by mass";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.01f, 0);
      var gravity = new Vector2(0, 0.09f * Mass);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = (float)GD.RandRange(5, 10);

      var size = GetViewportRect().Size;
      var xPos = (float)GD.RandRange(mover.BodySize, size.x - mover.BodySize);
      mover.Position = new Vector2(xPos, size.y / 2);

      AddChild(mover);
    }
  }
}
