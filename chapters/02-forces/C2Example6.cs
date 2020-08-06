using Godot;

public class C2Example6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.6:\n"
      + "Attraction";
  }

  public override void _Ready()
  {
    var size = GetViewport().Size;

    var attractor = new SimpleAttractor();
    attractor.Gravitation = 0.5f;
    attractor.Position = size / 2;
    AddChild(attractor);

    var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
    var xPos = (float)GD.RandRange(mover.BodySize, size.x - mover.BodySize);
    var yPos = (float)GD.RandRange(mover.BodySize, size.y - mover.BodySize);
    mover.Position = new Vector2(xPos, yPos);
    AddChild(mover);
  }
}
