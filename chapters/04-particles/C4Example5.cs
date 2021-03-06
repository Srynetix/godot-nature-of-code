using Godot;
using Drawing;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Example 4.5 - Multiple Particle Types.
    /// </summary>
    /// Spawn multiple particle types using ParticleCreationFunction.
    public class C4Example5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 4.5:\n"
              + "Multiple Particle Types";
        }

        private class ERoundParticle : SimpleFallingParticle
        {
            public ERoundParticle()
            {
                WrapMode = WrapModeEnum.Bounce;
                ForceRangeY = new Vector2(-0.15f, -0.15f);
                MeshSize = new Vector2(7.5f, 7.5f);
                Mesh.MeshType = SimpleMesh.TypeEnum.Circle;
            }
        }

        private class ESquareParticle : SimpleFallingParticle
        {
            public ESquareParticle()
            {
                WrapMode = WrapModeEnum.Bounce;
                ForceRangeY = new Vector2(0.15f, 0.15f);
                MeshSize = new Vector2(20f, 20f);
                Mesh.MeshType = SimpleMesh.TypeEnum.Square;
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var particleSystem = new SimpleParticleSystem()
            {
                ParticleCreationFunction = () =>
                {
                    SimpleParticle particle = null;

                    if (MathUtils.RandRangef(0, 1) >= 0.5)
                    {
                        particle = new ERoundParticle();
                    }
                    else
                    {
                        particle = new ESquareParticle();
                    }

                    particle.Lifespan = 2;
                    return particle;
                },
                Position = new Vector2(size.x / 2, size.y / 2)
            };
            AddChild(particleSystem);
        }
    }
}
