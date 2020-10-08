using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Example 4.3 - Particle System.
    /// </summary>
    /// Uses SimpleParticleSystem with its ParticleCreationFunction.
    public class C4Example3 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 4.3:\n"
              + "Particle System";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var particleSystem = new SimpleParticleSystem
            {
                ParticleCreationFunction = () =>
                {
                    var particle = new SimpleFallingParticle
                    {
                        WrapMode = SimpleMover.WrapModeEnum.Bounce,
                        MeshSize = new Vector2(20, 20),
                        Lifespan = 2
                    };
                    particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
                    return particle;
                },
                ParticleSpawnFrameDelay = 2,
                Position = new Vector2(size.x / 2, size.y / 4)
            };
            AddChild(particleSystem);
        }
    }
}
