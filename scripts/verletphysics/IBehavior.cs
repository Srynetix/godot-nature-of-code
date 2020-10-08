namespace VerletPhysics
{
    /// <summary>
    /// Behavior interface.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Apply behavior to a verlet point.
        /// </summary>
        /// <param name="point">Verlet point</param>
        /// <param name="delta">Delta time</param>
        void ApplyBehavior(VerletPoint point, float delta);
    }
}
