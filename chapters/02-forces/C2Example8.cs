using System.Linq;

using Godot;

public class C2Example8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.8:\n"
      + "Mutual Attraction";
  }

  public override void _Ready()
  {
    var size = GetViewport().Size;

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
      var bodySize = (float)GD.RandRange(5, 20);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
      var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
      mover.BodySize = bodySize;
      mover.Mass = mover.BodySize;
      mover.Position = new Vector2(xPos, yPos);

      // Add attractor on mover
      var attractor = new SimpleAttractor();
      attractor.Drawing = false;
      mover.AddChild(attractor);

      AddChild(mover);
    }
  }
}
