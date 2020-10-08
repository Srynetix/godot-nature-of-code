using Godot;

namespace Forces
{
    /// <summary>
    /// Simple repeller to push movers away.
    /// </summary>
    public class SimpleRepeller : SimpleAttractor
    {
        /// <summary>
        /// Generate attraction force depending on a mover.
        /// <summary>
        /// <param name="mover">Mover instance</param>
        /// <returns>Attraction force vector</returns>
        public override Vector2 Attract(SimpleMover mover)
        {
            return -base.Attract(mover);
        }
    }
}
