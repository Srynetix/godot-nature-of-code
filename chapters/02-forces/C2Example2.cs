using System.Linq;

using Godot;

public class C2Example2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.2:\n"
      + "Forces on many objects";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.05f, 0);
      var gravity = new Vector2(0, 0.2f);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      var bodySize = (float)GD.RandRange(5, 20);
      mover.BodySize = new Vector2(bodySize, bodySize);
      mover.Mass = (float)GD.RandRange(5, 10);
      AddChild(mover);
    }
  }
}
