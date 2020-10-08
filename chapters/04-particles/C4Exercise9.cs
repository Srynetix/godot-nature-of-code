using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Exercise 4.9 - Particle Repellers.
    /// </summary>
    /// Same principle as Example 4.7 but with multiple repellers.
    public class C4Exercise9 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 4.9:\n"
              + "Particle Repellers";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var particleSystem = new SimpleParticleSystem
            {
                Position = new Vector2(size.x / 2, size.y / 4),
                ParticleCreationFunction = () =>
                {
                    var particle = new SimpleFallingParticle
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

            var repeller = new SimpleRepeller
            {
                Position = particleSystem.Position + new Vector2(0, 100)
            };
            AddChild(repeller);

            var repeller2 = new SimpleRepeller
            {
                Position = particleSystem.Position + new Vector2(-100, 0)
            };
            AddChild(repeller2);
        }
    }
}
