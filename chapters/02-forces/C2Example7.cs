using System.Linq;

using Godot;

public class C2Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.7:\n"
      + "Attraction with many Movers";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var attractor = new SimpleAttractor();
    attractor.Gravitation = 0.5f;
    attractor.Position = size / 2;
    AddChild(attractor);

    foreach (var x in Enumerable.Range(0, 10))
    {
      var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = mover.BodySize;
      var xPos = (float)GD.RandRange(mover.BodySize, size.x - mover.BodySize);
      var yPos = (float)GD.RandRange(mover.BodySize, size.y - mover.BodySize);
      mover.Position = new Vector2(xPos, yPos);
      AddChild(mover);
    }
  }
}
