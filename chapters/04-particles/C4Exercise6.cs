using Godot;
using Forces;
using Particles;

namespace Examples.Chapter4
{
    /// <summary>
    /// Exercise 4.6 - Shattering Objects.
    /// </summary>
    /// Simple particle spawning on touch.
    public class C4Exercise6 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 4.6:\n"
              + "Shattering Objects\n\n"
              + "Touch screen to break objects";
        }

        private class EParticle : SimpleParticle
        {
            private bool initialForceDone = false;

            public EParticle()
            {
                WrapMode = WrapModeEnum.Bounce;
            }

            public override void _Ready()
            {
                base._Ready();

                // Initial velocity
                const int offset = 16;
                Velocity = new Vector2((float)GD.RandRange(-offset, offset), (float)GD.RandRange(-offset, offset));
                MeshSize *= (float)GD.RandRange(0.5f, 1.5f);
            }

            protected override void UpdateAcceleration()
            {
                if (!initialForceDone)
                {
                    const int offset = 16;
                    ApplyForce(new Vector2((float)GD.RandRange(-offset, offset), (float)GD.RandRange(-offset, offset)));
                    initialForceDone = true;
                }
                else
                {
                    ApplyForce(new Vector2(0, 0.281f));
                }
            }
        }

        private class ShatteringObject : SimpleMover
        {
            private SimpleParticleSystem particleSystem;
            private bool exploding = false;

            public override void _Ready()
            {
                base._Ready();

                particleSystem = new SimpleParticleSystem
                {
                    Emitting = false,
                    ParticleCountPerWave = 6,
                    ParticleCreationFunction = () =>
                    {
                        return new EParticle
                        {
                            MeshSize = MathUtils.RandRangef(0.5f, 1) * (MeshSize / 2),
                            InitialOffset = MathUtils.RandVector2(-1, 1, -1, 1).Normalized() * 10,
                            Velocity = MathUtils.RandVector2(-1, 1, -1, 1).Normalized() * 10,
                            Lifespan = 4
                        };
                    }
                };
                AddChild(particleSystem);
            }

            public void Explode()
            {
                if (exploding)
                {
                    return;
                }

                exploding = true;
                particleSystem.Emitting = true;
                particleSystem.ParticleCount = 6;
                Drawing = false;
            }

            public override void _UnhandledInput(InputEvent @event)
            {
                if (@event is InputEventScreenTouch eventScreenTouch)
                {
                    if (eventScreenTouch.Pressed && eventScreenTouch.Position.DistanceTo(GlobalPosition) < Radius)
                    {
                        Explode();
                    }
                }
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            const int objectCount = 10;
            float widthSlice = size.x / (objectCount + 1);
            for (int i = 0; i < objectCount; ++i)
            {
                var shatteringObject = new ShatteringObject
                {
                    Position = new Vector2((widthSlice * i) + (widthSlice / 2), (float)GD.RandRange(widthSlice, size.y - widthSlice)),
                    MeshSize = new Vector2(widthSlice, widthSlice) * (float)GD.RandRange(0.75f, 1.25f)
                };
                AddChild(shatteringObject);
            }
        }
    }
}
