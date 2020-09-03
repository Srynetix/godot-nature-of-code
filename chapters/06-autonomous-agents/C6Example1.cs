using Godot;
using Agents;
using Forces;

namespace Examples
{
  /// <summary>
  /// Chapter 6 - Autonomous Agents.
  /// </summary>
  namespace Chapter6
  {
    /// <summary>
    /// Example 6.1 - Seeking a target.
    /// </summary>
    /// Uses SimpleVehicle.
    public class C6Example1 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 6.1:\nSeeking a target";
      }

      private SimpleMover targetMover;

      #region Lifecycle methods

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        // Create target
        targetMover = new SimpleMover();
        targetMover.Position = size / 2;
        targetMover.Modulate = Colors.LightBlue.WithAlpha(128);
        AddChild(targetMover);

        // Create vehicle
        var vehicle = new SimpleVehicle();
        vehicle.Target = targetMover;
        vehicle.Position = size / 4;
        AddChild(vehicle);
      }

      public override void _Process(float delta)
      {
        targetMover.GlobalPosition = GetViewport().GetMousePosition();
      }

      #endregion
    }
  }
}
