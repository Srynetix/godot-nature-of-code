using Godot;
using Agents;
using Forces;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Exercise 6.3: Variable maximum speed.
    /// </summary>
    /// Use the distance between the mouse cursor and the vehicle to compute max speed and max force.
    public class C6Exercise3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 6.3:\nVariable maximum speed";
      }

      private SimpleMover targetMover;

      private class Vehicle : SimpleVehicle
      {
        protected override void UpdateAcceleration()
        {
          MaxVelocity = Mathf.Max(GlobalPosition.DistanceTo(Target.GlobalPosition) / 10, 4f);
          MaxForce = Mathf.Max(GlobalPosition.DistanceTo(Target.GlobalPosition) / 100, 0.1f);

          base.UpdateAcceleration();
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        // Create target
        targetMover = new SimpleMover();
        targetMover.Position = size / 2;
        targetMover.Modulate = Colors.LightBlue.WithAlpha(128);
        AddChild(targetMover);

        // Create vehicle
        var vehicle = new Vehicle();
        vehicle.Target = targetMover;
        vehicle.Position = size / 4;
        AddChild(vehicle);
      }

      public override void _Process(float delta)
      {
        targetMover.GlobalPosition = GetViewport().GetMousePosition();
      }
    }
  }
}
