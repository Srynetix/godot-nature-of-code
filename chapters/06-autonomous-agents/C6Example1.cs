using Godot;
using Forces;
using Drawing;

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
    /// Update SimpleMover acceleration depending on mouse position.
    public class C6Example1 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 6.1:\nSeeking a target.";
      }

      private SimpleMover targetMover;

      private class Vehicle : SimpleMover
      {
        public Node2D Target;
        public float MaxForce = 0.1f;

        public Vehicle(Node2D target)
        {
          Mesh.MeshType = SimpleMesh.TypeEnum.Square;
          Mesh.MeshSize = new Vector2(40, 20);

          SyncRotationOnVelocity = true;
          MaxVelocity = 4;
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
          Seek(Target.GlobalPosition);
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
