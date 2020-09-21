using Godot;

namespace VerletPhysics
{
    /// <summary>
    /// Simple verlet ragdoll.
    /// </summary>
    public class VerletRagdoll
    {
        private readonly VerletPoint head;
        private readonly VerletPoint shoulder;
        private readonly VerletPoint elbowLeft;
        private readonly VerletPoint elbowRight;
        private readonly VerletPoint handLeft;
        private readonly VerletPoint handRight;
        private readonly VerletPoint pelvis;
        private readonly VerletPoint kneeLeft;
        private readonly VerletPoint kneeRight;
        private readonly VerletPoint footLeft;
        private readonly VerletPoint footRight;

        /// <summary>
        /// Create a verlet ragdoll.
        /// </summary>
        /// <param name="world">Verlet world</param>
        /// <param name="centerPosition">Center position</param>
        /// <param name="height">Body height</param>
        /// <param name="gravityScale">Gravity scale</param>
        /// <param name="tearSensitivityFactor">Distance factor required to break links. Use '-1' to create an unbreakable ragdoll</param>
        /// <param name="pointRadius">Point radius</param>
        /// <param name="drawIntermediatePoints">Draw all ragdoll points</param>
        /// <param name="drawSupportLinks">Draw support links</param>
        public VerletRagdoll(VerletWorld world, Vector2 centerPosition, float height, float gravityScale = 1, float tearSensitivityFactor = -1, float pointRadius = 10f, bool drawIntermediatePoints = false, bool drawSupportLinks = false)
        {
            Vector2 genPos()
            {
                return centerPosition + MathUtils.RandVector2(-5, 5, -5, 5);
            }

            VerletPoint createPoint(float mass, float? radius = null, bool? visible = null)
            {
                var point = world.CreatePoint(genPos(), mass: mass, radius: radius ?? pointRadius, visible: visible ?? drawIntermediatePoints);
                point.GravityScale = gravityScale;
                return point;
            }

            var headSize = pointRadius * 3;
            var handSize = pointRadius * 2;
            var footSize = pointRadius * 1.5f;
            var headLength = height / 7.5f;

            head = createPoint(4, radius: headSize, visible: true);
            shoulder = createPoint(26);
            world.CreateLink(head, shoulder, restingDistance: 5 / 4 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);

            elbowLeft = createPoint(2);
            elbowRight = createPoint(2);
            world.CreateLink(elbowLeft, shoulder, restingDistance: 3 / 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);
            world.CreateLink(elbowRight, shoulder, restingDistance: 3 / 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);

            handLeft = createPoint(2, radius: handSize, visible: true);
            handRight = createPoint(2, radius: handSize, visible: true);
            world.CreateLink(handLeft, elbowLeft, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);
            world.CreateLink(handRight, elbowRight, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);

            pelvis = createPoint(15);
            world.CreateLink(pelvis, shoulder, restingDistance: 3.5f * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 0.8f);
            world.CreateLink(pelvis, head, restingDistance: 4.75f * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 0.02f, visible: drawSupportLinks, color: Colors.LightBlue.WithAlpha(64));

            kneeLeft = createPoint(10);
            kneeRight = createPoint(10);
            world.CreateLink(kneeLeft, pelvis, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);
            world.CreateLink(kneeRight, pelvis, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);

            footLeft = createPoint(20, radius: footSize, visible: true);
            footRight = createPoint(20, radius: footSize, visible: true);
            world.CreateLink(footLeft, kneeLeft, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);
            world.CreateLink(footRight, kneeRight, restingDistance: 2 * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 1);
            world.CreateLink(footLeft, shoulder, restingDistance: 7.5f * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 0.001f, visible: drawSupportLinks, color: Colors.LightBlue.WithAlpha(64));
            world.CreateLink(footRight, shoulder, restingDistance: 7.5f * headLength, tearSensitivityFactor: tearSensitivityFactor, stiffness: 0.001f, visible: drawSupportLinks, color: Colors.LightBlue.WithAlpha(64));
        }
    }
}
