using Godot;

namespace VerletPhysics
{
    /// <summary>
    /// Gravity behavior.
    /// </summary>
    public class GravityBehavior : IBehavior
    {
        /// <summary>Gravity value</summary>
        public Vector2 Gravity = new Vector2(0, 9.81f);

        /// <summary>
        /// Create a gravity behavior.
        /// </summary>
        /// <param name="gravity">Gravity value</param>
        public GravityBehavior(Vector2? gravity = null)
        {
            Gravity = gravity ?? Gravity;
        }

        public void ApplyBehavior(VerletPoint point, float delta)
        {
            if (!point.Touched)
            {
                point.ApplyForce(Gravity * point.GravityScale);
            }
        }
    }
}
