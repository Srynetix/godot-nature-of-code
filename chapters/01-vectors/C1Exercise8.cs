using Godot;

public class C1Exercise8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 1.8:\n"
      + "Try implementing the above example with a variable magnitude of acceleration, stronger when it is either closer or farther away.";
  }

  public class Mover : SimpleMover
  {
    protected override void UpdateAcceleration()
    {
      var mousePos = GetViewport().GetMousePosition();
      var distanceVec = mousePos - Position;
      var distanceLength = distanceVec.Length();
      var dir = distanceVec.Normalized();

      var coef = Mathf.Clamp(1.0f / distanceLength * 10f, 0.25f, 5f);
      Acceleration = dir * (1.0f / distanceLength) * 10f;
    }
  }

  private Mover mover;

  public override void _Ready()
  {
    GD.Randomize();
    var size = GetViewport().Size;

    mover = new Mover();
    mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
    AddChild(mover);
  }
}
