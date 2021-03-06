using System.Linq;

using Godot;
using Drawing;
using Forces;
using Oscillation;
using Particles;

namespace Ecosystem
{
    /// <summary>
    /// The Ecosystem project.
    /// </summary>
    public class Ecosystem : Control
    {
        /// <summary>
        /// Base abstract lifeform. Based on SimpleMover.
        /// </summary>
        public class Lifeform : SimpleMover
        {
            /// <summary>Debug draw enabled?</summary>
            public bool DebugDrawEnabled;

            public Lifeform() : base(WrapModeEnum.Wrap)
            {
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
                Acceleration = Vector2.Zero;
                AngularAcceleration = 0;
                MaxVelocity = 5f;
                MaxAngularVelocity = 0.25f;

                // Disable internal mesh by using custom draw and no custom draw function
                Mesh.MeshType = SimpleMesh.TypeEnum.Custom;
            }

            public override void _Ready()
            {
                base._Ready();
                SetAtRandomAngle();
            }

            public override void _Draw()
            {
                if (DebugDrawEnabled)
                {
                    DebugDraw();
                }
            }

            private void DebugDraw()
            {
                DrawCircle(Vector2.Zero, 1f, Colors.Red);
                DrawLine(Vector2.Zero, Vector2.Right * 10, Colors.Blue);
            }

            private void SetAtRandomAngle()
            {
                Rotation = MathUtils.Map((float)GD.RandRange(0, 1), 0, 1, 0, 2 * Mathf.Pi);
            }
        }

        /// <summary>
        /// Base fly lifeform.
        /// </summary>
        public class BaseFly : Lifeform
        {
            /// <summary>Acceleration factor</summary>
            public float AccelerationFactor = 0.5f;

            /// <summary>Angular acceleration factor</summary>
            public float AngularAccelerationFactor = 0.01f;

            /// <summary>Base color</summary>
            public Color BaseColor = Colors.White;

            /// <summary>Wing rotation factor</summary>
            public float WingRotationFactor = 0.5f;

            /// <summary>Wing speed</summary>
            public float WingSpeed = 64f;

            /// <summary>Wing size</summary>
            public float WingSize = 3f;

            /// <summary>Wing alpha color</summary>
            public byte WingColorAlpha = 80;

            /// <summary>Wings time</summary>
            protected float tWings;

            /// <summary>Body sprite</summary>
            protected SimpleCircleSprite sBody;

            /// <summary>Left wing sprite</summary>
            protected SimpleCircleSprite sLeftWing;

            /// <summary>Right wing sprite</summary>
            protected SimpleCircleSprite sRightWing;

            public BaseFly()
            {
                MeshSize = new Vector2(4, 4);
                MaxVelocity = 5f;
                MaxAngularVelocity = 0.01f;

                sBody = new SimpleCircleSprite();
                sLeftWing = new SimpleCircleSprite();
                sRightWing = new SimpleCircleSprite();
            }

            public override void _Ready()
            {
                base._Ready();

                var wingsColor = BaseColor.WithAlpha(WingColorAlpha);
                sBody.Radius = Radius;
                sBody.Modulate = BaseColor;
                sLeftWing.Radius = WingSize;
                sLeftWing.Modulate = wingsColor;
                sRightWing.Radius = WingSize;
                sRightWing.Modulate = wingsColor;

                AddChild(sBody);
                AddChild(sLeftWing);
                AddChild(sRightWing);
            }

            public override void _Process(float delta)
            {
                base._Process(delta);

                tWings += delta * WingSpeed;

                // Update pos
                var leftWingPos = (Vector2.Up * (Radius + 1)).Rotated(Mathf.Sin(tWings) * WingRotationFactor);
                var rightWingPos = (Vector2.Up * -(Radius + 1)).Rotated(Mathf.Sin(-tWings) * WingRotationFactor);
                sLeftWing.Position = leftWingPos;
                sRightWing.Position = rightWingPos;
            }
        }

        /// <summary>
        /// Attracted fly.
        /// Uses SimpleAttractor.
        /// </summary>
        public class AttractedFly : BaseFly
        {
            /// <summary>
            /// Create an attracted fly.
            /// </summary>
            public AttractedFly()
            {
                BaseColor = Colors.Olive;
            }

            protected override void UpdateAcceleration()
            {
                AngularAcceleration = MathUtils.SignedRandf() * AngularAccelerationFactor;
            }

            public override void _Ready()
            {
                base._Ready();

                var attractor = new SimpleAttractor() { Visible = false };
                AddChild(attractor);
            }
        }

        /// <summary>
        /// Nervous fly. Fly everywhere nervously spawning particles.
        /// Uses ParticleSystem.
        /// </summary>
        public class NervousFly : BaseFly
        {
            private SimpleParticleSystem particleSystem;

            /// <summary>
            /// Create a nervous fly.
            /// </summary>
            public NervousFly()
            {
                BaseColor = Colors.MediumPurple;
            }

            protected override void UpdateAcceleration()
            {
                AngularAcceleration = MathUtils.SignedRandf() * AngularAccelerationFactor;
                Acceleration = new Vector2(
                    MathUtils.SignedRandf(),
                    MathUtils.SignedRandf()
                ) * AccelerationFactor;
            }

            public override void _Ready()
            {
                base._Ready();

                particleSystem = new SimpleParticleSystem()
                {
                    ShowBehindParent = true,
                    LocalCoords = false,
                    ParticlesContainer = GetParent(),
                    ParticleCreationFunction = () =>
                    {
                        return new SimpleFallingParticle()
                        {
                            ShowBehindParent = true,
                            MeshSize = new Vector2(2.5f, 2.5f),
                            WrapMode = WrapModeEnum.None,
                            ForceRangeX = new Vector2(-0.005f, 0.005f),
                            ForceRangeY = new Vector2(-0.005f, 0.005f),
                            Lifespan = 1
                        };
                    }
                };

                AddChild(particleSystem);
            }
        }

        /// <summary>
        /// Nervous butterfly. Same behavior than nervous fly.
        /// Uses multiple SimpleOscillator.
        /// </summary>
        public class NervousButterfly : BaseFly
        {
            private class OscillatingWing : SimpleOscillator
            {
                public OscillatingWing()
                {
                    Radius = 4;
                    Velocity = new Vector2(0f, 0.75f);
                    Amplitude = new Vector2(1, 7);
                    ShowLine = false;
                    BallColor = Colors.Green.WithAlpha(128);
                    ShowBehindParent = true;
                }
            }

            /// <summary>
            /// Create a nervous butterfly.
            /// </summary>
            public NervousButterfly()
            {
                BaseColor = Colors.GreenYellow;
                sLeftWing.Visible = false;
                sRightWing.Visible = false;
            }

            protected override void UpdateAcceleration()
            {
                AngularAcceleration = MathUtils.SignedRandf() * AngularAccelerationFactor;
                Acceleration = new Vector2(
                    MathUtils.SignedRandf(),
                    MathUtils.SignedRandf()
                ) * AccelerationFactor;
            }

            public override void _Ready()
            {
                base._Ready();

                var oscillatingLeftWing = new OscillatingWing()
                {
                    Position = Vector2.Left * (Radius + 1)
                };
                AddChild(oscillatingLeftWing);

                var oscillatingRightWing = new OscillatingWing()
                {
                    Position = Vector2.Right * (Radius + 1)
                };
                AddChild(oscillatingRightWing);
            }
        }

        /// <summary>
        /// Swimming fish.
        /// </summary>
        public class SwimmingFish : Lifeform
        {
            /// <summary>Tail speed</summary>
            public float TailSpeed = 10f;

            /// <summary>Base color</summary>
            public Color BaseColor = Colors.CadetBlue;

            /// <summary>Forward acceleration</summary>
            public float ForwardAcceleration = 0.25f;

            /// <summary>Side offset acceleration</summary>
            public float SideOffsetAcceleration = 0.1f;

            private float tTail;
            private readonly SimpleCircleSprite tail1;
            private readonly SimpleCircleSprite tail2;
            private readonly SimpleCircleSprite tail3;
            private readonly SimpleCircleSprite body1;
            private readonly SimpleCircleSprite body2;
            private readonly SimpleCircleSprite body3;
            private readonly SimpleCircleSprite body4;
            private readonly SimpleCircleSprite leftWing;
            private readonly SimpleCircleSprite rightWing;

            /// <summary>
            /// Create a swimming fish.
            /// </summary>
            public SwimmingFish()
            {
                MaxVelocity = 1f;

                tail1 = new SimpleCircleSprite();
                tail2 = new SimpleCircleSprite();
                tail3 = new SimpleCircleSprite();
                body1 = new SimpleCircleSprite();
                body2 = new SimpleCircleSprite();
                body3 = new SimpleCircleSprite();
                body4 = new SimpleCircleSprite();
                leftWing = new SimpleCircleSprite();
                rightWing = new SimpleCircleSprite();

                Color colorToUse = BaseColor;
                Color lightenedColor = colorToUse.Lightened(0.25f);
                Color lowDarkenedColor = colorToUse.Darkened(0.1f);
                Color midDarkenedColor = colorToUse.Darkened(0.25f);
                tail1.Modulate = colorToUse.Darkened(0.5f);
                tail2.Modulate = midDarkenedColor;
                tail3.Modulate = lowDarkenedColor;
                tail1.Radius = 2;
                tail2.Radius = 2;
                tail3.Radius = 2;
                body1.Modulate = colorToUse;
                body2.Modulate = colorToUse;
                body3.Modulate = colorToUse;
                body4.Modulate = colorToUse;
                body1.Radius = 3;
                body2.Radius = 4;
                body3.Radius = 5;
                body4.Radius = 4;
                leftWing.Modulate = lightenedColor;
                rightWing.Modulate = lightenedColor;
                leftWing.Radius = 1.5f;
                rightWing.Radius = 1.5f;
            }

            protected override void UpdateAcceleration()
            {
                var forward = Vector2.Right.Rotated(Rotation).Normalized() * ForwardAcceleration;
                var offset = new Vector2(MathUtils.SignedRandf(), MathUtils.SignedRandf()) * SideOffsetAcceleration;
                Acceleration = forward + offset;
            }

            public override void _Ready()
            {
                base._Ready();

                AddChild(tail1);
                AddChild(tail2);
                AddChild(tail3);
                AddChild(body1);
                AddChild(body2);
                AddChild(body3);
                AddChild(body4);
                AddChild(leftWing);
                AddChild(rightWing);
            }

            public override void _Process(float delta)
            {
                base._Process(delta);

                tTail += delta * TailSpeed;
                float tailAngle = Mathf.Sin(tTail);

                // Tail
                tail1.Position = (Vector2.Left * 2).Rotated(tailAngle);
                tail2.Position = Vector2.Left.Rotated(tailAngle);
                tail3.Position = Vector2.Right.Rotated(-tailAngle);

                // Body
                body1.Position = Vector2.Right * 3;
                body2.Position = Vector2.Right * 6;
                body3.Position = Vector2.Right * 10;
                body4.Position = Vector2.Right * 13;

                // 'Wings'
                leftWing.Position = (Vector2.Right * (10 + tailAngle)) + (Vector2.Up * 5);
                rightWing.Position = (Vector2.Right * (10 + tailAngle)) + (Vector2.Down * 5);
            }
        }

        /// <summary>
        /// Hopping bunny.
        /// </summary>
        public class HoppingBunny : Lifeform
        {
            /// <summary>Jump speed</summary>
            public float JumpSpeed = 8f;

            /// <summary>Tail speed</summary>
            public float TailSpeed = 10f;

            /// <summary>Acceleration factor</summary>
            public float AccelerationFactor = 0.025f;

            /// <summary>Base color</summary>
            public Color BaseColor = Colors.White;

            /// <summary>Eye color</summary>
            public Color EyeColor = Colors.LightBlue;

            /// <summary>Ear rotation factor</summary>
            public float EarRotationFactor = 0.01f;

            /// <summary>Tail rotation factor</summary>
            public float TailRotationFactor = 0.05f;

            private float tJump;
            private float tTail;
            private readonly SimpleCircleSprite body1;
            private readonly SimpleCircleSprite body2;
            private readonly SimpleCircleSprite paws1;
            private readonly SimpleCircleSprite paws2;
            private readonly SimpleCircleSprite paws3;
            private readonly SimpleCircleSprite paws4;
            private readonly SimpleCircleSprite tail;
            private readonly SimpleCircleSprite leftEar1;
            private readonly SimpleCircleSprite leftEar2;
            private readonly SimpleCircleSprite leftEar3;
            private readonly SimpleCircleSprite leftEar4;
            private readonly SimpleCircleSprite leftEar5;
            private readonly SimpleCircleSprite head;
            private readonly SimpleCircleSprite eye;
            private readonly SimpleCircleSprite nose;
            private readonly SimpleCircleSprite rightEar1;
            private readonly SimpleCircleSprite rightEar2;
            private readonly SimpleCircleSprite rightEar3;
            private readonly SimpleCircleSprite rightEar4;
            private readonly SimpleCircleSprite rightEar5;

            /// <summary>
            /// Create a hopping bunny.
            /// </summary>
            public HoppingBunny()
            {
                body1 = new SimpleCircleSprite();
                body2 = new SimpleCircleSprite();
                paws1 = new SimpleCircleSprite();
                paws2 = new SimpleCircleSprite();
                paws3 = new SimpleCircleSprite();
                paws4 = new SimpleCircleSprite();
                tail = new SimpleCircleSprite();
                leftEar1 = new SimpleCircleSprite();
                leftEar2 = new SimpleCircleSprite();
                leftEar3 = new SimpleCircleSprite();
                leftEar4 = new SimpleCircleSprite();
                leftEar5 = new SimpleCircleSprite();
                head = new SimpleCircleSprite();
                eye = new SimpleCircleSprite();
                nose = new SimpleCircleSprite();
                rightEar1 = new SimpleCircleSprite();
                rightEar2 = new SimpleCircleSprite();
                rightEar3 = new SimpleCircleSprite();
                rightEar4 = new SimpleCircleSprite();
                rightEar5 = new SimpleCircleSprite();

                Color baseColor = BaseColor;
                Color darkenedColor = baseColor.Darkened(0.1f);

                // Body
                body1.Modulate = baseColor;
                body2.Modulate = baseColor;
                body1.Radius = 10;
                body2.Radius = 10;

                // Paws
                paws1.Modulate = darkenedColor;
                paws2.Modulate = darkenedColor;
                paws3.Modulate = darkenedColor;
                paws4.Modulate = darkenedColor;
                paws1.Radius = 2;
                paws2.Radius = 2;
                paws3.Radius = 2;
                paws4.Radius = 2;

                // Tail
                tail.Modulate = darkenedColor;
                tail.Radius = 4;

                // Left ear
                leftEar1.Modulate = darkenedColor;
                leftEar2.Modulate = darkenedColor;
                leftEar3.Modulate = darkenedColor;
                leftEar4.Modulate = darkenedColor;
                leftEar5.Modulate = darkenedColor;
                leftEar1.Radius = 3;
                leftEar2.Radius = 3;
                leftEar3.Radius = 3;
                leftEar4.Radius = 3;
                leftEar5.Radius = 3;

                // Head
                head.Modulate = baseColor;
                head.Radius = 8;

                // Eye
                eye.Modulate = EyeColor;
                eye.Radius = 2;

                // Nose
                nose.Modulate = baseColor;
                nose.Radius = 1.5f;

                // Right ear
                rightEar1.Modulate = darkenedColor;
                rightEar2.Modulate = darkenedColor;
                rightEar3.Modulate = darkenedColor;
                rightEar4.Modulate = darkenedColor;
                rightEar5.Modulate = darkenedColor;
                rightEar1.Radius = 3;
                rightEar2.Radius = 3;
                rightEar3.Radius = 3;
                rightEar4.Radius = 3;
                rightEar5.Radius = 3;
            }

            protected override void UpdateAcceleration()
            {
                Acceleration.y = Mathf.Sin(tJump);
                Acceleration.x = AccelerationFactor * Mathf.Sign(Scale.x);
            }

            public override void _Ready()
            {
                base._Ready();

                Rotation = 0;

                // Random direction left or right
                if ((float)GD.RandRange(0, 1) < 0.5f)
                {
                    Scale = new Vector2(-Scale.x, Scale.y);
                }

                // Random initial point in time
                tJump = (float)GD.RandRange(0f, 100f);

                AddChild(body1);
                AddChild(body2);
                AddChild(paws1);
                AddChild(paws2);
                AddChild(paws3);
                AddChild(paws4);
                AddChild(tail);
                AddChild(leftEar1);
                AddChild(leftEar2);
                AddChild(leftEar3);
                AddChild(leftEar4);
                AddChild(leftEar5);
                AddChild(head);
                AddChild(eye);
                AddChild(nose);
                AddChild(rightEar1);
                AddChild(rightEar2);
                AddChild(rightEar3);
                AddChild(rightEar4);
                AddChild(rightEar5);
            }

            public override void _Process(float delta)
            {
                base._Process(delta);

                tTail += delta * TailSpeed;
                tJump += delta * JumpSpeed;

                float tailAngle = Mathf.Sin(tTail) * TailRotationFactor;
                float earAngle = Mathf.Sin(tTail) * EarRotationFactor;

                // Body
                body1.Position = Vector2.Zero;
                body2.Position = Vector2.Right * 7.5f;

                // Paws
                paws1.Position = (Vector2.Down * 9f) + Vector2.Left;
                paws2.Position = (Vector2.Down * 9f) + (Vector2.Right * 2f);
                paws3.Position = (Vector2.Down * 9f) + (Vector2.Right * 10f);
                paws4.Position = (Vector2.Down * 9f) + (Vector2.Right * 12f);

                // Tail
                tail.Position = ((Vector2.Left * 10f) + (Vector2.Up * 6)).Rotated(tailAngle);

                // Left ear
                leftEar1.Position = (Vector2.Right * 12f) + (Vector2.Up * 14);
                leftEar2.Position = (Vector2.Right * 12f) + (Vector2.Up * 16);
                leftEar3.Position = (Vector2.Right * 12f) + (Vector2.Up * 18);
                leftEar4.Position = ((Vector2.Right * 12f) + (Vector2.Up * 20)).Rotated(-earAngle);
                leftEar5.Position = ((Vector2.Right * 13f) + (Vector2.Up * 22)).Rotated(-earAngle);

                // Head
                head.Position = (Vector2.Right * 8f) + (Vector2.Up * 8);

                // Eye
                eye.Position = (Vector2.Right * 12f) + (Vector2.Up * 10);

                // Nose
                nose.Position = (Vector2.Right * 16f) + (Vector2.Up * 8);

                // Right ear
                rightEar1.Position = (Vector2.Right * 5f) + (Vector2.Up * 14);
                rightEar2.Position = (Vector2.Right * 5f) + (Vector2.Up * 16);
                rightEar3.Position = (Vector2.Right * 5f) + (Vector2.Up * 18);
                rightEar4.Position = ((Vector2.Right * 5f) + (Vector2.Up * 20)).Rotated(earAngle);
                rightEar5.Position = ((Vector2.Right * 6f) + (Vector2.Up * 22)).Rotated(earAngle);
            }
        }

        /// <summary>Entity count per species</summary>
        public int CountPerSpecies = 5;

        private Control drawZone;
        private const int zoneCount = 5;
        private const float zoneMargin = 10;

        public override void _Ready()
        {
            drawZone = GetNode<Control>("DrawZone");

            foreach (int _ in Enumerable.Range(0, CountPerSpecies))
            {
                var fly = new NervousFly()
                {
                    Scale = Vector2.One * (float)GD.RandRange(1f, 2f)
                };
                AddInZone(fly, 4);

                var butterfly = new NervousButterfly()
                {
                    Scale = Vector2.One * (float)GD.RandRange(1f, 2f)
                };
                AddInZone(butterfly, 3);

                var fish = new SwimmingFish()
                {
                    Scale = Vector2.One * (float)GD.RandRange(1f, 2f)
                };
                AddInZone(fish, 0);

                var bunny = new HoppingBunny()
                {
                    Scale = Vector2.One * (float)GD.RandRange(1f, 2f)
                };
                AddInZone(bunny, 1);

                var attractedFly = new AttractedFly()
                {
                    Scale = Vector2.One * (float)GD.RandRange(1f, 2f)
                };
                AddInZone(attractedFly, 2);
            }
        }

        private void AddInZone(Lifeform lifeform, int zoneIdx)
        {
            var size = GetViewportRect().Size;
            var zoneSplit = size.y / zoneCount;
            var zoneLowerLimit = size.y - (zoneMargin + (zoneIdx * zoneSplit));
            var zoneUpperLimit = zoneLowerLimit - zoneSplit;

            var xPosition = (float)GD.RandRange(lifeform.MeshSize.x, size.x - lifeform.MeshSize.x);
            var yPosition = (float)GD.RandRange(zoneLowerLimit, zoneUpperLimit);

            lifeform.Position = new Vector2(xPosition, yPosition);
            drawZone.AddChild(lifeform);
        }
    }
}
