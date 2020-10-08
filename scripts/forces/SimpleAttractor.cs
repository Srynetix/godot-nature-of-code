using Godot;
using Drawing;

/// <summary>
/// Force-related primitives.
/// </summary>
namespace Forces
{
    /// <summary>
    /// Simple mover attractor.
    /// </summary>
    public class SimpleAttractor : SimpleCircleSprite
    {
        /// <summary>Attractor mass</summary>
        public float Mass = 20.0f;

        /// <summary>Gravitaton</summary>
        public float Gravitation = 1.0f;

        /// <summary>Minimum force</summary>
        public float MinForce = 5;

        /// <summary>Maximum force</summary>
        public float MaxForce = 25;

        /// <summary>
        /// Create a default attractor with a 20px radius.
        /// </summary>
        public SimpleAttractor()
        {
            Radius = 20f;
            Modulate = Colors.LightGoldenrod;
            Visible = true;
        }

        /// <summary>
        /// Generate attraction force depending on a mover.
        /// <summary>
        /// <param name="mover">Mover instance</param>
        /// <returns>Attraction force vector</returns>
        public virtual Vector2 Attract(SimpleMover mover)
        {
            var force = GlobalPosition - mover.GlobalPosition;
            var length = Mathf.Clamp(force.Length(), MinForce, MaxForce);
            float strength = (Gravitation * Mass * mover.Mass) / (length * length);
            return force.Normalized() * strength;
        }

        public override void _Ready()
        {
            base._Ready();
            AddToGroup("attractors");
        }

        public override void _Process(float delta)
        {
            AttractNodes();
        }

        private void AttractNodes()
        {
            // For each mover
            foreach (var n in GetTree().GetNodesInGroup("movers"))
            {
                // Ignore parent mover
                var parent = GetParent();
                if (parent != null && parent == n)
                {
                    continue;
                }

                var mover = (SimpleMover)n;
                var force = Attract(mover);
                mover.ApplyForce(force);
            }
        }
    }
}
