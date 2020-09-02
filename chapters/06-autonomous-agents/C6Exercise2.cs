using Godot;
using Forces;
using Drawing;

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

      private class Vehicle : SimpleMover
      {
        public SimpleMover Target;
        public float MaxForce = 0.8f;

        public Vehicle(SimpleMover target)
        {
          Mesh.MeshType = SimpleMesh.TypeEnum.Square;
          Mesh.MeshSize = new Vector2(40, 20);

          SyncRotationOnVelocity = true;
          MaxVelocity = 8;
          Target = target;
        }

        public void Seek(Vector2 target)
        {
          var targetForce = (target - GlobalPosition).Normalized() * MaxVelocity;
          var steerForce = (targetForce - Velocity).Clamped(MaxForce);
          ApplyForce(steerForce);
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
        var vehicle = new Vehicle(targetMover);
        vehicle.Position = size / 4;
        AddChild(vehicle);
      }

      public override void _Process(float delta)
      {
        UpdateTargetPosition(GetViewport().GetMousePosition());
      }

      #endregion

      #region Private methods

      private void UpdateTargetPosition(Vector2 targetPosition)
      {
        targetMover.GlobalPosition = targetPosition;
      }

      #endregion
    }
  }
}
