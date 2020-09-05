using Godot;
using Forces;

namespace Examples.Chapter1
{
  /// <summary>
  /// Example 1.11 - Movers towards mouse.
  /// </summary>
  /// Uses same principle as Example 1.10 mover but with multiple occurences.
  public class C1Example11 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 1.11:\n"
        + "Movers towards mouse";
    }

    private class Mover : SimpleMover
    {
      protected override void UpdateAcceleration()
      {
        var mousePos = GetViewport().GetMousePosition();
        var dir = (mousePos - Position).Normalized();

        Acceleration = dir * 0.5f;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var moverCount = 20;
      for (int i = 0; i < moverCount; ++i)
      {
        var mover = new Mover();
        mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
        AddChild(mover);
      }
    }
  }
}
