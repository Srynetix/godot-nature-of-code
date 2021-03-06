using Godot;
using Drawing;
using Forces;

namespace Oscillation
{
    /// <summary>
    /// Simple spring.
    /// Spring anchor is the node position.
    /// Manage children as targets.
    /// </summary>
    public class SimpleSpring : Node2D
    {
        /// <summary>Spring length</summary>
        public float Length = 100;

        /// <summary>K coefficient</summary>
        public float K = 0.2f;

        /// <summary>Minimal length</summary>
        public float MinLength = 50;

        /// <summary>Maximal length</summary>
        public float MaxLength = 150;

        private SimpleMover currentMover;
        private int touchIndex = -1;
        private bool touched;
        private readonly SimpleLineSprite lineSprite;

        /// <summary>
        /// Create a default spring.
        /// </summary>
        public SimpleSpring()
        {
            lineSprite = new SimpleLineSprite() { Width = 2 };
        }

        /// <summary>
        /// Set mover at initial position.
        /// </summary>
        /// <param name="mover">Mover</param>
        /// <param name="initialPosition">Initial position</param>
        public void SetMover(SimpleMover mover, Vector2 initialPosition)
        {
            if (currentMover != null)
            {
                RemoveChild(currentMover);
                currentMover.QueueFree();
            }

            currentMover = mover;
            AddChild(currentMover);

            // Set position
            currentMover.Position = initialPosition;
        }

        public override void _Ready()
        {
            lineSprite.PositionA = GlobalPosition;
            lineSprite.PositionB = GlobalPosition;
            AddChild(lineSprite);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (currentMover == null)
            {
                return;
            }

            if (@event is InputEventScreenTouch eventScreenTouch)
            {
                if (eventScreenTouch.Pressed && touchIndex == -1 && eventScreenTouch.Position.DistanceTo(currentMover.GlobalPosition) < currentMover.Radius * 2)
                {
                    touchIndex = eventScreenTouch.Index;
                    currentMover.DisableForces = true;
                    touched = true;
                }

                if (!eventScreenTouch.Pressed && touchIndex == eventScreenTouch.Index)
                {
                    currentMover.DisableForces = false;
                    touched = false;
                    touchIndex = -1;
                }
            }
            else if (@event is InputEventScreenDrag eventScreenDrag)
            {
                if (touched)
                {
                    currentMover.GlobalPosition = eventScreenDrag.Position;
                }
            }
        }

        public override void _Process(float delta)
        {
            currentMover?.ApplyForce(ComputeForce(currentMover));
            ConstrainLength();

            if (currentMover != null)
            {
                // Update line
                var color = (touched) ? Colors.LightGoldenrod : Colors.LightGray;
                lineSprite.PositionA = GlobalPosition;
                lineSprite.PositionB = currentMover.GlobalPosition;
                lineSprite.Modulate = color;
            }
        }

        private Vector2 ComputeForce(SimpleMover mover)
        {
            if (touched)
            {
                return Vector2.Zero;
            }

            var force = mover.Position;
            var length = force.Length();
            var stretch = length - Length;

            return force.Normalized() * -K * stretch;
        }

        private void ConstrainLength()
        {
            if (currentMover == null)
            {
                return;
            }

            var dir = currentMover.Position;
            var d = dir.Length();

            if (d < MinLength)
            {
                currentMover.Position = dir.Normalized() * MinLength;
                currentMover.Velocity = Vector2.Zero;
            }
            else if (d > MaxLength)
            {
                currentMover.Position = dir.Normalized() * MaxLength;
                currentMover.Velocity = Vector2.Zero;
            }
        }
    }
}
