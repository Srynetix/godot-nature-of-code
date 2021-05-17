using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
    /// <summary>
    /// Simple verlet world which handle simulation book-keeping.
    /// </summary>
    public class VerletWorld : Node2D
    {
        /// <summary>Constraint resolution accuracy</summary>
        public int ConstraintAccuracy = 2;

        private readonly List<IBehavior> behaviors;
        private readonly List<VerletPoint> points;
        private readonly List<VerletLink> linksToRemove;

        /// <summary>
        /// Create a default verlet world.
        /// </summary>
        public VerletWorld()
        {
            points = new List<VerletPoint>();
            linksToRemove = new List<VerletLink>();
            behaviors = new List<IBehavior>();
        }

        /// <summary>
        /// Create a verlet point.
        /// </summary>
        /// <param name="initialPosition">Initial position</param>
        /// <param name="mass">Mass</param>
        /// <param name="gravityScale">Gravity scale</param>
        /// <param name="radius">Radius</param>
        /// <param name="color">Color</param>
        /// <param name="visible">Show point</param>
        /// <returns>Verlet point</returns>
        public VerletPoint CreatePoint(Vector2? initialPosition = null, float? mass = null, float? gravityScale = null, float? radius = null, Color? color = null, bool? visible = null)
        {
            var point = new VerletPoint(this);
            points.Add(point);
            AddChild(point);

            point.Mass = mass ?? point.Mass;
            point.GravityScale = gravityScale ?? point.GravityScale;
            point.Radius = radius ?? point.Radius;
            point.Visible = visible ?? point.Visible;
            point.Modulate = color ?? point.Modulate;

            if (initialPosition.HasValue)
            {
                point.MoveToPosition(initialPosition.Value);
            }

            return point;
        }

        /// <summary>
        /// Create a verlet link.
        /// </summary>
        /// <param name="a">First verlet point</param>
        /// <param name="b">First verlet point</param>
        /// <param name="restingDistance">Resting distance</param>
        /// <param name="minimalDistance">Minimal link distance. Use `-1` to disable.</param>
        /// <param name="maximalDistance">Maximal link distance. Use `-1` to disable.</param>
        /// <param name="tearSensitivity">Distance required to break the link. Use `-1` to create an unbreakable link.</param>
        /// <param name="tearSensitivityFactor">Distance factor required to break the link. Use `-1` to create an unbreakable link.</param>
        /// <param name="stiffness">Stiffness of the link</param>
        /// <param name="width">Width of the link</param>
        /// <param name="color">Link color</param>
        /// <param name="visible">Show link</param>
        /// <returns>Verlet link</returns>
        public VerletLink CreateLink(
          VerletPoint a,
          VerletPoint b,
          float? restingDistance = null,
          float? minimalDistance = null,
          float? maximalDistance = null,
          float? tearSensitivity = null,
          float? tearSensitivityFactor = null,
          float? stiffness = null,
          float? width = null,
          Color? color = null,
          bool? visible = null)
        {
            var link = new VerletLink(this, a, b);
            a.AddLink(link);
            AddChild(link);

            link.Visible = visible ?? link.Visible;
            link.RestingDistance = restingDistance ?? link.RestingDistance;
            link.MinimalDistance = minimalDistance ?? link.MinimalDistance;
            link.MaximalDistance = maximalDistance ?? link.MaximalDistance;
            link.TearSensitivity = tearSensitivity ?? link.TearSensitivity;
            link.Stiffness = stiffness ?? link.Stiffness;
            link.Modulate = color ?? link.Modulate;
            link.Width = width ?? link.Width;

            if (tearSensitivityFactor.HasValue)
            {
                link.TearSensitivity = link.RestingDistance * tearSensitivityFactor.Value;
            }

            return link;
        }

        /// <summary>
        /// Mark a link to be removed next frame.
        /// </summary>
        /// <param name="link">Verlet link</param>
        public void QueueLinkRemoval(VerletLink link)
        {
            if (!linksToRemove.Contains(link))
            {
                linksToRemove.Add(link);
            }
        }

        /// <summary>
        /// Add a new behavior.
        /// </summary>
        /// <param name="behavior">Behavior</param>
        public void AddBehavior(IBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        /// <summary>
        /// Remove an existing behavior.
        /// </summary>
        /// <param name="behavior">Behavior</param>
        public void RemoveBehavior(IBehavior behavior)
        {
            behaviors.Remove(behavior);
        }

        public override void _PhysicsProcess(float delta)
        {
            ProcessPoints(delta);

            foreach (VerletLink link in linksToRemove)
            {
                RemoveLink(link);
            }
            linksToRemove.Clear();
        }

        private void RemoveLink(VerletLink link)
        {
            foreach (VerletPoint p in points)
            {
                p.RemoveLink(link);
            }

            RemoveChild(link);
        }

        private void ProcessPoints(float delta)
        {
            var size = GetViewportRect().Size;

            // Apply constraints
            for (int i = 0; i < ConstraintAccuracy; ++i)
            {
                foreach (VerletPoint point in points)
                {
                    point.Constraint(size);
                }
            }

            // Apply behaviors
            foreach (IBehavior behavior in behaviors)
            {
                foreach (VerletPoint point in points)
                {
                    behavior.ApplyBehavior(point, delta);
                }
            }

            // Update point positions
            foreach (VerletPoint point in points)
            {
                point.UpdateMovement(delta);
            }
        }
    }
}
