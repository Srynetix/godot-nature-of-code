using Godot;
using Forces;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Exercise 6.2: Pursuit.
    /// </summary>
    /// Same principle as Example 6.1, but using the target velocity to predict its position.
    public class C6Exercise2 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 6.2:\nPursuit";
      }

      private SimpleMover targetMover;

      private class Vehicle : SimpleVehicle
      {
        public Vehicle()
        {
          MaxVelocity = 8;
          MaxForce = 0.8f;
        }

        protected override void UpdateAcceleration()
        {
          Seek(Target.GlobalPosition + Target.Velocity);
        }
      }

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
        var vehicle = new Vehicle();
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
