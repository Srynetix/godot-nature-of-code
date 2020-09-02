using Godot;
using Forces;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Example 6.3: Stay Within Walls.
    /// </summary>
    /// Detect bounds when updating acceleration.
    /// This example can also be used for the Exercise 6.5.
    public class C6Example3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 6.3:\nStay Within Walls";
      }

      public float ScreenOffset = 50;

      private class Vehicle : SimpleVehicle
      {
        public float ScreenOffset;
        private Vector2 arbitraryTarget;

        public Vehicle()
        {
          // Forward
          arbitraryTarget = new Vector2(10, 0);
          // Disable wrapping
          WrapMode = WrapModeEnum.None;

          // Initial velocity
          Velocity = new Vector2(MaxVelocity, MaxVelocity);
          MaxVelocity = 3;
          MaxForce = 0.3f;
        }

        protected override void UpdateAcceleration()
        {
          DetectBounds();
        }

        private void DetectBounds()
        {
          var size = GetViewportRect().Size;
          if (GlobalPosition.x >= size.x - ScreenOffset)
          {
            var desired = new Vector2(-MaxVelocity, Velocity.y);
            var steer = (desired - Velocity).Clamped(MaxForce);
            ApplyForce(steer);
          }

          else if (GlobalPosition.x <= ScreenOffset)
          {
            var desired = new Vector2(MaxVelocity, Velocity.y);
            var steer = (desired - Velocity).Clamped(MaxForce);
            ApplyForce(steer);
          }

          else if (GlobalPosition.y >= size.y - ScreenOffset)
          {
            var desired = new Vector2(Velocity.x, -MaxVelocity);
            var steer = (desired - Velocity).Clamped(MaxForce);
            ApplyForce(steer);
          }

          else if (GlobalPosition.y <= ScreenOffset)
          {
            var desired = new Vector2(Velocity.x, MaxVelocity);
            var steer = (desired - Velocity).Clamped(MaxForce);
            ApplyForce(steer);
          }
        }
      }

      #region Lifecycle methods

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var vehicle = new Vehicle();
        vehicle.ScreenOffset = ScreenOffset;
        vehicle.Position = size / 2;
        AddChild(vehicle);
      }

      public override void _Draw()
      {
        var size = GetViewportRect().Size;
        DrawRect(new Rect2(new Vector2(ScreenOffset, ScreenOffset), size.x - ScreenOffset * 2, size.y - ScreenOffset * 2), Colors.White.WithAlpha(8));
      }

      #endregion
    }
  }
}
