using Godot;
using Forces;

namespace Examples.Chapter1
{
  /// <summary>
  /// Exercise 1.8 - Acceleration towards mouse variant.
  /// </summary>
  /// Uses a custom SimpleMover with a specific UpdateAcceleration,
  /// with the distance between the mover and the mouse as an acceleration factor.
  public class C1Exercise8 : Node2D, IExample
  {
    public string GetSummary()
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

        Acceleration = dir * (1.0f / distanceLength) * 10f;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var mover = new Mover {
        Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y))
      };

      AddChild(mover);
    }
  }
}
