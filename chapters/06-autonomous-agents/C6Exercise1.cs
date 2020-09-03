using Godot;
using Agents;
using Forces;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Exercise 6.1 - Fleeing a target.
    /// </summary>
    /// Same principle than Example 6.1, but with a flee behavior.
    public class C6Exercise1 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 6.1:\n Fleeing a target";
      }

      private SimpleMover targetMover;

      private class Vehicle : SimpleVehicle
      {
        public float FleeDistance = 200;

        public void FleeTarget()
        {
          if (Target != null)
          {
            var targetForce = (Target.GlobalPosition - GlobalPosition).Normalized() * MaxVelocity;
            var steerForce = (-targetForce - Velocity).Clamped(MaxForce);
            ApplyForce(steerForce);
          }
        }

        protected override void UpdateAcceleration()
        {
          // Depending on the distance, use a different behavior
          if (GlobalPosition.DistanceSquaredTo(Target.GlobalPosition) > FleeDistance * FleeDistance)
          {
            SeekTarget();
          }
          else
          {
            FleeTarget();
          }
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
