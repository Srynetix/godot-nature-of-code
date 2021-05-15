using Godot;
using Drawing;
using Forces;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Exercise 4.2 - Particle with Angular Velocity.
    /// </summary>
    /// Use SimpleMover angular velocity at the core of SimpleParticle.
    public class C4Exercise2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 4.2:\n"
              + "Particle with Angular Velocity";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var particle = new SimpleFallingParticle()
            {
                WrapMode = SimpleMover.WrapModeEnum.Bounce,
                Position = size / 2,
                MeshSize = new Vector2(20, 20),
                Lifespan = 4
            };
            particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
            AddChild(particle);
        }
    }
}
