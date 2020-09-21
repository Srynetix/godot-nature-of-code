using Godot;
using Forces;

namespace Examples.Chapter3
{
    /// <summary>
    /// Exercise 3.2 - Cannon.
    /// </summary>
    /// Simple cannon using _Draw, spawning custom SimpleMovers.
    /// Uses a Timer to limit projectile spawning.
    public class C3Exercise2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 3.2:\n"
              + "Cannon";
        }

        private class Cannon : Node2D
        {
            public float BasisSize = 20;
            public float MovementSpeed = 1;

            private float t = 0;

            public override void _Draw()
            {
                var bodyWidth = BasisSize * 2;
                var bodyLength = bodyWidth * 2;

                // Basis
                DrawRect(new Rect2(0, -bodyWidth / 2, bodyLength, bodyWidth), Colors.Purple);
                DrawRect(new Rect2(2, -(bodyWidth - 4) / 2, bodyLength - 4, bodyWidth - 4), Colors.White);
                DrawCircle(Vector2.Zero, BasisSize, Colors.LightBlue);
                DrawCircle(Vector2.Zero, BasisSize - 2, Colors.White);
            }

            public Vector2 GetSpawnPoint()
            {
                var spawnPointLength = (BasisSize * 4) - 25;
                return new Vector2(spawnPointLength, 0).Rotated(Rotation) + GlobalPosition;
            }

            public override void _Process(float delta)
            {
                Rotation = Mathf.Deg2Rad(-45 + (Mathf.Sin(t) * 45));
                t += delta * MovementSpeed;
            }

            public void Fire()
            {
                var spawnPoint = GetSpawnPoint();

                var proj = new Projectile
                {
                    Position = spawnPoint,
                    Rotation = Rotation
                };
                GetParent().AddChild(proj);
            }
        }

        private class Projectile : SimpleMover
        {
            public bool Fired = false;

            public Projectile() : base(WrapModeEnum.Bounce)
            {
                MaxVelocity = 100f;
                MaxAngularVelocity = 20f;
            }

            protected override void UpdateAcceleration()
            {
                if (!Fired)
                {
                    // Boom
                    var direction = Vector2.Right.Rotated(Rotation).Normalized();
                    ApplyForce(direction * 40f);
                    Fired = true;
                }

                AngularAcceleration = Acceleration.x / 10.0f;

                // Gravity
                ApplyForce(new Vector2(0, 0.981f));
                ApplyFriction(0.25f);
                ApplyAngularFriction(0.25f);
            }
        }

        private Timer timer;
        private Cannon cannon;

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            const int offset = 50;
            const float fireTime = 1f;

            timer = new Timer
            {
                WaitTime = fireTime,
                Autostart = true
            };
            AddChild(timer);

            timer.Connect("timeout", this, nameof(CannonFire));

            cannon = new Cannon
            {
                Position = new Vector2(offset, size.y - offset)
            };
            AddChild(cannon);
        }

        private void CannonFire()
        {
            cannon.Fire();
        }
    }
}
