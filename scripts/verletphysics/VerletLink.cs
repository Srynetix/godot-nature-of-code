using Godot;
using Drawing;

namespace VerletPhysics
{
    /// <summary>
    /// Verlet breakable link.
    /// </summary>
    public class VerletLink : SimpleLineSprite
    {
        /// <summary>Resting distance</summary>
        public float RestingDistance;

        /// <summary>Minimal distance. Use `-1` to disable.</summary>
        public float MinimalDistance = -1;

        /// <summary>Maximal distance. Use `-1` to disable.</summary>
        public float MaximalDistance = -1;

        /// <summary>Stiffness of the link, between 0 and 1.</summary>
        public float Stiffness = 1;

        /// <summary>Distance required to break the link. Use `-1` to create an unbreakable link.</summary>
        public float TearSensitivity = -1;

        /// <summary>First verlet point</summary>
        public VerletPoint A;

        /// <summary>Second verlet point</summary>
        public VerletPoint B;

        private readonly VerletWorld world;

        /// <summary>
        /// Create an uninitialized verlet link.
        /// </summary>
        public VerletLink()
        {
            this.world = null;
        }

        /// <summary>
        /// Create a simple verlet link.
        /// </summary>
        /// <param name="world">World</param>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        public VerletLink(VerletWorld world, VerletPoint a, VerletPoint b)
        {
            A = a;
            B = b;
            this.world = world;

            PositionA = A.GlobalPosition;
            PositionB = B.GlobalPosition;

            if (RestingDistance == 0)
            {
                // Calculate resting distance from points position
                RestingDistance = (PositionB - PositionA).Length();
            }
        }

        /// <summary>
        /// Apply link constraint on the two verlet points.
        /// </summary>
        public void Constraint()
        {
            var diff = A.GlobalPosition - B.GlobalPosition;
            var d = diff.Length();
            var difference = (RestingDistance - d) / d;

            // Check for tear
            if (TearSensitivity > 0 && d > TearSensitivity)
            {
                world.QueueLinkRemoval(this);
            }

            // Check for min value
            if (MinimalDistance > 0 && d >= MinimalDistance)
            {
                // Do nothing
                return;
            }

            var imA = 1 / A.Mass;
            var imB = 1 / B.Mass;
            var scalarA = (imA / (imA + imB)) * Stiffness;
            var scalarB = Stiffness - scalarA;

            Vector2 computeMovement(float scalar)
            {
                var movement = diff * difference * scalar;
                if (MaximalDistance > 0)
                {
                    return movement.Clamped(MaximalDistance);
                }
                else
                {
                    return movement;
                }
            }

            A.GlobalPosition += computeMovement(scalarA);
            B.GlobalPosition -= computeMovement(scalarB);

            PositionA = A.GlobalPosition;
            PositionB = B.GlobalPosition;
        }

        public override void _Process(float delta)
        {
            PositionA = A.GlobalPosition;
            PositionB = B.GlobalPosition;
        }
    }
}
