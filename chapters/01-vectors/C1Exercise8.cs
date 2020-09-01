using Godot;
using Forces;

namespace Examples
{
  /// <summary>
  /// Exercise 1.8 - Acceleration towards mouse variant.
  /// </summary>
  /// Uses a custom SimpleMover with a specific UpdateAcceleration,
  /// with the distance between the mover and the mouse as an acceleration factor.
  public class C1Exercise8 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 1.8:\n"
        + "Accel. towards mouse variant";
    }

    private class Mover : SimpleMover
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

    public override void _Ready()
    {
      GD.Randomize();
      var size = GetViewportRect().Size;

      var mover = new Mover();
      mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
      AddChild(mover);
    }
  }
}
