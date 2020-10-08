using Godot;
using Drawing;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Exercise 4.5 - Particle Systems Lifespan.
    /// </summary>
    /// Remove a particle system once the particle count is reached.
    public class C4Exercise5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 4.5:\n"
              + "Particle Systems Lifespan\n\n"
              + "Touch screen to spawn particle system";
        }

        private void AddParticleSystem(Vector2 position)
        {
            var ps = new SimpleParticleSystem
            {
                ParticleCount = 200,
                RemoveWhenEmptyParticles = true,
                ParticleCreationFunction = () =>
                {
                    var particle = new SimpleFallingParticle
                    {
                        Lifespan = 2,
                        MeshSize = new Vector2(10, 10)
                    };
                    particle.Mesh.MeshType = SimpleMesh.TypeEnum.Square;
                    return particle;
                },
                GlobalPosition = position
            };
            AddChild(ps);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventScreenTouch eventScreenTouch)
            {
                if (eventScreenTouch.Pressed)
                {
                    AddParticleSystem(eventScreenTouch.Position);
                }
            }
        }

        public override void _Ready()
        {
            // Initial systems
            var size = GetViewportRect().Size;
            const int initialCount = 2;
            float splitSize = size.x / initialCount;

            for (int i = 0; i < initialCount; ++i)
            {
                AddParticleSystem(new Vector2((splitSize / 2) + (splitSize * i), size.y / 4));
            }
        }
    }
}
