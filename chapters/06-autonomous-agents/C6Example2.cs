using Godot;
using Agents;
using Forces;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.2: Arrive steering behavior.
  /// </summary>
  /// Use SimpleVehicle's ArriveDistance property.
  public class C6Example2 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 6.2:\nArrive steering behavior";
    }

    private SimpleMover targetMover;

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      // Create target
      targetMover = new SimpleMover {
        Position = size / 2,
        Modulate = Colors.LightBlue.WithAlpha(128)
      };
      AddChild(targetMover);

      // Create vehicle
      var vehicle = new SimpleVehicle {
        Target = targetMover,
        Position = size / 4,
        ArriveDistance = 100
      };
      AddChild(vehicle);
    }

    public override void _Process(float delta)
    {
      targetMover.GlobalPosition = GetViewport().GetMousePosition();
    }
  }
}
