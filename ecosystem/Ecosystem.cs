using System.Linq;

using Godot;

public class Ecosystem : Control
{
  public class Lifeform : SimpleMover
  {
    public bool DebugDraw = false;

    public Lifeform() : base(WrapModeEnum.Wrap)
    {
      Velocity = Vector2.Zero;
      AngularVelocity = 0;
      Acceleration = Vector2.Zero;
      AngularAcceleration = 0;
      MaxVelocity = 5f;
      MaxAngularVelocity = 0.25f;
    }

    public override void _Ready()
    {
      base._Ready();

      SetAtRandomScreenPos();
      SetAtRandomAngle();
    }

    public void SetAtRandomScreenPos()
    {
      var size = GetViewportRect().Size;
      Position = new Vector2((float)GD.RandRange(0, 1) * size.x, (float)GD.RandRange(0, 1) * size.y);
    }

    public void SetAtRandomAngle()
    {
      Rotation = Utils.Map((float)GD.RandRange(0, 1), 0, 1, 0, 2 * Mathf.Pi);
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

  public class AttractedFly : Lifeform
  {
    public float AngularAccelerationFactor = 0.01f;
    public float AccelerationFactor = 0.5f;
    public Color BaseColor = Colors.Olive;
    public float WingRotationFactor = 0.5f;
    public float WingSpeed = 64f;
    public float WingSize = 3f;
    public byte WingColorAlpha = 80;

    private float tWings = 0;

    public AttractedFly()
    {
      BodySize = new Vector2(4, 4);
      MaxVelocity = 5f;
      MaxAngularVelocity = 0.01f;
    }

    public override void _Ready()
    {
      base._Ready();

      var attractor = new SimpleAttractor();
      attractor.Drawing = false;
      AddChild(attractor);
    }

    public override void _DrawLifeform()
    {
      // Body
      DrawCircle(Vector2.Zero, Radius, BaseColor);

      var leftWingPos = (Vector2.Up * (Radius + 1)).Rotated(Mathf.Sin(tWings) * WingRotationFactor);
      var rightWingPos = (Vector2.Up * -(Radius + 1)).Rotated(Mathf.Sin(-tWings) * WingRotationFactor);

      // Wings
      var wingsColor = BaseColor.WithAlpha(WingColorAlpha);
      DrawCircle(leftWingPos, WingSize, wingsColor);
      DrawCircle(rightWingPos, WingSize, wingsColor);
    }

    protected override void UpdateAcceleration()
    {
      AngularAcceleration = Utils.SignedRandf() * AngularAccelerationFactor;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      tWings += delta * WingSpeed;
    }
  }

  public class NervousFly : Lifeform
  {
    public float AngularAccelerationFactor = 0.01f;
    public float AccelerationFactor = 0.5f;
    public Color BaseColor = Colors.MediumPurple;
    public float WingRotationFactor = 0.5f;
    public float WingSpeed = 64f;
    public float WingSize = 3f;
    public byte WingColorAlpha = 80;

    private float tWings = 0;

    public NervousFly()
    {
      BodySize = new Vector2(4f, 4f);
      MaxVelocity = 5f;
      MaxAngularVelocity = 0.01f;
    }

    public override void _DrawLifeform()
    {
      // Body
      DrawCircle(Vector2.Zero, Radius, BaseColor);

      var leftWingPos = (Vector2.Up * (Radius + 1)).Rotated(Mathf.Sin(tWings) * WingRotationFactor);
      var rightWingPos = (Vector2.Up * -(Radius + 1)).Rotated(Mathf.Sin(-tWings) * WingRotationFactor);

      // Wings
      var wingsColor = BaseColor.WithAlpha(WingColorAlpha);
      DrawCircle(leftWingPos, WingSize, wingsColor);
      DrawCircle(rightWingPos, WingSize, wingsColor);
    }

    protected override void UpdateAcceleration()
    {
      AngularAcceleration = Utils.SignedRandf() * AngularAccelerationFactor;
      Acceleration = new Vector2(
          Utils.SignedRandf(),
          Utils.SignedRandf()
      ) * AccelerationFactor;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);
      tWings += delta * WingSpeed;
    }
  }

  public class NervousButterfly : NervousFly
  {
    public class OscillatingWing : SimpleOscillator
    {
      public override void _Ready()
      {
        Radius = 4;
        Velocity = new Vector2(0f, 0.75f);
        Amplitude = new Vector2(1, 7);
        ShowLine = false;
        BallOutlineColor = Colors.Green;

        ShowBehindParent = true;
      }
    }

    public NervousButterfly()
    {
      BaseColor = Colors.GreenYellow;
    }

    public override void _Ready()
    {
      base._Ready();

      var oscillatingLeftWing = new OscillatingWing();
      oscillatingLeftWing.Position = Vector2.Left * (Radius + 1);
      AddChild(oscillatingLeftWing);

      var oscillatingRightWing = new OscillatingWing();
      oscillatingRightWing.Position = Vector2.Right * (Radius + 1);
      AddChild(oscillatingRightWing);
    }

    public override void _DrawLifeform()
    {
      DrawCircle(Vector2.Zero, Radius, BaseColor);
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
      MaxVelocity = 1f;
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

    protected override void UpdateAcceleration()
    {
      var forward = Vector2.Right.Rotated(Rotation).Normalized() * ForwardAcceleration;
      var offset = new Vector2(Utils.SignedRandf(), Utils.SignedRandf()) * SideOffsetAcceleration;
      Acceleration = forward + offset;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      tTail += delta * TailSpeed;
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
      if ((float)GD.RandRange(0, 1) < 0.5f)
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

    protected override void UpdateAcceleration()
    {
      Acceleration.y = Mathf.Sin(tJump);
      Acceleration.x = AccelerationFactor * Mathf.Sign(Scale.x);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      tTail += delta * TailSpeed;
      tJump += delta * JumpSpeed;
    }
  }

  public int CountPerSpecies = 3;

  private Control drawZone;

  public override void _Ready()
  {
    GD.Randomize();
    drawZone = GetNode<Control>("DrawZone");

    foreach (int x in Enumerable.Range(0, CountPerSpecies))
    {
      var fly = new NervousFly();
      fly.Scale = Vector2.One * (float)GD.RandRange(0.5f, 2f);
      drawZone.AddChild(fly);

      var butterfly = new NervousButterfly();
      butterfly.Scale = Vector2.One * (float)GD.RandRange(0.5f, 2f);
      drawZone.AddChild(butterfly);

      var fish = new SwimmingFish();
      fish.Scale = Vector2.One * (float)GD.RandRange(0.5f, 2f);
      drawZone.AddChild(fish);

      var bunny = new HoppingBunny();
      bunny.Scale = Vector2.One * (float)GD.RandRange(0.5f, 1.5f);
      drawZone.AddChild(bunny);

      var attractedFly = new AttractedFly();
      attractedFly.Scale = Vector2.One * (float)GD.RandRange(0.5f, 1.5f);
      drawZone.AddChild(attractedFly);
    }
  }
}
