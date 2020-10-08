using Godot;

namespace Physics
{
    /// <summary>
    /// Simple simulated mouse joint.
    /// </summary>
    public class SimpleMouseJoint : Node2D
    {
        /// <summary>Joint speed</summary>
        public float Speed = 2;

        private bool active = false;
        private RigidBody2D parent = null;

        /// <summary>
        /// Is joint active?
        /// </summary>
        /// <returns>True/False</returns>
        public virtual bool IsActive()
        {
            return active;
        }

        /// <summary>
        /// Compute target position.
        /// </summary>
        /// <returns>Position vector</returns>
        protected virtual Vector2 ComputeTargetPosition()
        {
            return GetViewport().GetMousePosition();
        }

        public override void _Ready()
        {
            parent = (RigidBody2D)GetParent();
        }

        public override void _Process(float delta)
        {
            if (IsActive())
            {
                // Apply to parent object
                var r = ComputeTargetPosition() - parent.Position;
                parent.LinearVelocity = r * Speed;
            }

            Update();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventScreenTouch eventScreenTouch)
            {
                active = eventScreenTouch.Pressed;
            }
        }

        public override void _Draw()
        {
            if (IsActive())
            {
                DrawLine(Vector2.Zero, (ComputeTargetPosition() - parent.GlobalPosition).Rotated(-parent.GlobalRotation), Colors.Gray, 2);
            }
        }
    }
}
