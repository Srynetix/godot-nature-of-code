using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Example 4.7 - Particle Repeller.
    /// </summary>
    /// Combine SimpleRepeller (based on SimpleAttractor) and SimpleFallingParticle (based on SimpleParticle and SimpleMover).
    public class C4Example7 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 4.7:\n"
              + "Particle Repeller";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var particleSystem = new SimpleParticleSystem()
            {
                Position = new Vector2(size.x / 2, size.y / 4),
                ParticleCreationFunction = () =>
                {
                    var particle = new SimpleFallingParticle()
                    {
                        MeshSize = new Vector2(20, 20),
                        Lifespan = 2,
                        Mass = 2
                    };
                    particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
                    return particle;
                }
            };
            AddChild(particleSystem);

            var repeller = new SimpleRepeller()
            {
                Position = new Vector2(size.x / 2, size.y / 2)
            };
            AddChild(repeller);
        }
    }
}
