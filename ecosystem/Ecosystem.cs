using Godot;
using System.Linq;

public class Ecosystem : Control
{
    public class Lifeform : Node2D
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float AngularVelocity;
        public float AngularAcceleration;
        public float TopSpeed;
        public float TopAngularSpeed;

        public bool DebugDraw = false;

        public Lifeform()
        {
            Velocity = Vector2.Zero;
            AngularVelocity = 0;
            Acceleration = Vector2.Zero;
            AngularAcceleration = 0;
            TopSpeed = 5f;
            TopAngularSpeed = 0.25f;
        }

        public override void _Ready()
        {
            SetAtRandomScreenPos();
            SetAtRandomAngle();
        }

        public void Move()
        {
            AngularVelocity = Mathf.Clamp(AngularVelocity + AngularAcceleration, -TopAngularSpeed, TopAngularSpeed);
            Rotation += AngularVelocity;

            Velocity = (Velocity + Acceleration).Clamped(TopSpeed);
            Position += Velocity;
            WrapEdges();
        }

        private void WrapEdges()
        {
            var size = GetViewport().Size;

            if (Position.x > size.x)
            {
                Position = new Vector2(0, Position.y);
            }
            else if (Position.x < 0)
            {
                Position = new Vector2(size.x, Position.y);
            }

            if (Position.y > size.y)
            {
                Position = new Vector2(Position.x, 0);
            }
            else if (Position.y < 0)
            {
                Position = new Vector2(Position.x, size.y);
            }
        }

        public void SetAtRandomScreenPos()
        {
            var size = GetViewport().Size;
            Position = new Vector2(GD.Randf() * size.x, GD.Randf() * size.y);
        }

        public void SetAtRandomAngle()
        {
            Rotation = Utils.Map(GD.Randf(), 0, 1, 0, 2 * Mathf.Pi);
        }

        public void _DebugDraw()
        {
            DrawCircle(Vector2.Zero, 1f, Colors.Red);
            DrawLine(Vector2.Zero, Vector2.Right * 10, Colors.Blue);
        }

        public virtual void _DrawLifeform()
        {
            // Nothing
        }

        public override void _Draw()
        {
            _DrawLifeform();

            if (DebugDraw)
            {
                _DebugDraw();
            }
        }
    }

    public class NervousFly : Lifeform
    {
        public float AngularAccelerationFactor = 0.01f;
        public float AccelerationFactor = 0.5f;
        public float BodySize = 4f;
        public Color BaseColor = Colors.MediumPurple;
        public float WingRotationFactor = 0.5f;
        public float WingSpeed = 64f;
        public float WingSize = 3f;
        public byte WingColorAlpha = 80;

        private float tWings = 0;

        public NervousFly()
        {
            TopSpeed = 5f;
            TopAngularSpeed = 0.01f;
        }

        public override void _DrawLifeform()
        {
            // Body
            DrawCircle(Vector2.Zero, BodySize, BaseColor);

            var leftWingPos = (Vector2.Up * (BodySize + 1)).Rotated(Mathf.Sin(tWings) * WingRotationFactor);
            var rightWingPos = (Vector2.Up * -(BodySize + 1)).Rotated(Mathf.Sin(-tWings) * WingRotationFactor);

            // Wings
            var wingsColor = BaseColor.WithAlpha(WingColorAlpha);
            DrawCircle(leftWingPos, WingSize, wingsColor);
            DrawCircle(rightWingPos, WingSize, wingsColor);
        }

        public override void _Process(float delta)
        {
            AngularAcceleration = Utils.SignedRandf() * AngularAccelerationFactor;
            Acceleration = new Vector2(
                Utils.SignedRandf(),
                Utils.SignedRandf()
            ) * AccelerationFactor;

            Move();

            tWings += delta * WingSpeed;

            Update();
        }
    }

    public class SwimmingFish : Lifeform
    {
        public float TailSpeed = 10f;
        public Color BaseColor = Colors.CadetBlue;
        public float ForwardAcceleration = 0.25f;
        public float SideOffsetAcceleration = 0.1f;

        private float tTail = 0;

        public SwimmingFish()
        {
            TopSpeed = 1f;
        }

        public override void _DrawLifeform()
        {
            Color colorToUse = BaseColor;

            Color lightenedColor = colorToUse.Lightened(0.25f);
            Color lowDarkenedColor = colorToUse.Darkened(0.1f);
            Color midDarkenedColor = colorToUse.Darkened(0.25f);
            Color highDarkenedColor = colorToUse.Darkened(0.5f);

            float tailAngle = Mathf.Sin(tTail);

            // Tail
            DrawCircle((Vector2.Left * 2).Rotated(tailAngle), 2, highDarkenedColor);
            DrawCircle(Vector2.Left.Rotated(tailAngle), 2, midDarkenedColor);
            DrawCircle(Vector2.Right.Rotated(-tailAngle), 2, lowDarkenedColor);

            // Body
            DrawCircle(Vector2.Right * 3, 3, colorToUse);
            DrawCircle(Vector2.Right * 6, 4, colorToUse);
            DrawCircle(Vector2.Right * 10, 5, colorToUse);
            DrawCircle(Vector2.Right * 13, 4, colorToUse);

            // 'Wings'
            DrawCircle(Vector2.Right * (10 + tailAngle) + Vector2.Up * 5, 1.5f, lightenedColor);
            DrawCircle(Vector2.Right * (10 + tailAngle) + Vector2.Down * 5, 1.5f, lightenedColor);
        }

        public override void _Process(float delta)
        {
            var forward = Vector2.Right.Rotated(Rotation).Normalized() * ForwardAcceleration;
            var offset = new Vector2(Utils.SignedRandf(), Utils.SignedRandf()) * SideOffsetAcceleration;
            Acceleration = forward + offset;

            Move();

            tTail += delta * TailSpeed;

            Update();
        }
    }

    public class HoppingBunny : Lifeform
    {
        public float JumpSpeed = 8f;
        public float TailSpeed = 10f;
        public float AccelerationFactor = 0.025f;
        public Color BaseColor = Colors.White;
        public Color EyeColor = Colors.LightBlue;
        public float EarRotationFactor = 0.01f;
        public float TailRotationFactor = 0.05f;

        private float tJump;
        private float tTail;

        public override void _Ready()
        {
            base._Ready();
            Rotation = 0;

            // Random direction left or right
            if (GD.Randf() < 0.5f)
            {
                Scale = new Vector2(-Scale.x, Scale.y);
            }

            // Random initial point in time
            tJump = (float)GD.RandRange(0f, 100f);
        }

        public override void _DrawLifeform()
        {
            float tailAngle = Mathf.Sin(tTail) * TailRotationFactor;
            float earAngle = Mathf.Sin(tTail) * EarRotationFactor;

            Color baseColor = BaseColor;
            Color darkenedColor = baseColor.Darkened(0.1f);

            // Body
            DrawCircle(Vector2.Zero, 10, baseColor);
            DrawCircle(Vector2.Right * 7.5f, 10, baseColor);

            // Paws
            DrawCircle(Vector2.Down * 9f + Vector2.Left, 2, darkenedColor);
            DrawCircle(Vector2.Down * 9f + Vector2.Right * 2f, 2, darkenedColor);
            DrawCircle(Vector2.Down * 9f + Vector2.Right * 10f, 2, darkenedColor);
            DrawCircle(Vector2.Down * 9f + Vector2.Right * 12f, 2, darkenedColor);

            // Tail
            DrawCircle((Vector2.Left * 10f + Vector2.Up * 6).Rotated(tailAngle), 4, darkenedColor);

            // Left ear
            DrawCircle(Vector2.Right * 12f + Vector2.Up * 14, 3, darkenedColor);
            DrawCircle(Vector2.Right * 12f + Vector2.Up * 16, 3, darkenedColor);
            DrawCircle(Vector2.Right * 12f + Vector2.Up * 18, 3, darkenedColor);
            DrawCircle((Vector2.Right * 12f + Vector2.Up * 20).Rotated(-earAngle), 3, darkenedColor);
            DrawCircle((Vector2.Right * 13f + Vector2.Up * 22).Rotated(-earAngle), 3, darkenedColor);

            // Head
            DrawCircle(Vector2.Right * 8f + Vector2.Up * 8, 8, baseColor);

            // Eye
            DrawCircle(Vector2.Right * 12f + Vector2.Up * 10, 2, EyeColor);

            // Nose
            DrawCircle(Vector2.Right * 16f + Vector2.Up * 8, 1.5f, baseColor);

            // Right ear
            DrawCircle(Vector2.Right * 5f + Vector2.Up * 14, 3, darkenedColor);
            DrawCircle(Vector2.Right * 5f + Vector2.Up * 16, 3, darkenedColor);
            DrawCircle(Vector2.Right * 5f + Vector2.Up * 18, 3, darkenedColor);
            DrawCircle((Vector2.Right * 5f + Vector2.Up * 20).Rotated(earAngle), 3, darkenedColor);
            DrawCircle((Vector2.Right * 6f + Vector2.Up * 22).Rotated(earAngle), 3, darkenedColor);
        }

        public override void _Process(float delta)
        {
            Acceleration.y = Mathf.Sin(tJump);
            Acceleration.x = AccelerationFactor * Mathf.Sign(Scale.x);

            tTail += delta * TailSpeed;
            tJump += delta * JumpSpeed;

            Move();
            Update();
        }
    }

    private Label fpsLabel;

    public override void _Ready()
    {
        GD.Randomize();

        fpsLabel = GetNode<Label>("MarginContainer/VBoxContainer/FPS");

        int nervousFlyCount = 10;
        foreach (int x in Enumerable.Range(0, nervousFlyCount))
        {
            var fly = new NervousFly();
            fly.Scale = Vector2.One * (float)GD.RandRange(0.5f, 2f);
            AddChild(fly);
        }

        int swimmingFishCount = 10;
        foreach (int x in Enumerable.Range(0, swimmingFishCount))
        {
            var fish = new SwimmingFish();
            fish.Scale = Vector2.One * (float)GD.RandRange(0.5f, 2f);
            AddChild(fish);
        }

        int hoppingBunnyCount = 10;
        foreach (int x in Enumerable.Range(0, hoppingBunnyCount))
        {
            var bunny = new HoppingBunny();
            bunny.Scale = Vector2.One * (float)GD.RandRange(0.5f, 1.5f);
            AddChild(bunny);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (!inputEventKey.Pressed)
            {
                if (inputEventKey.Scancode == (int)KeyList.Space)
                {
                    GetTree().ReloadCurrentScene();
                }
            }
        }
    }

    public override void _Process(float delta)
    {
        fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
    }
}
