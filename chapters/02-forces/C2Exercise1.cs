using System.Linq;

using Godot;

public class C2Exercise1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.1:\n"
      + "Using forces, simulate a helium-filled balloon floating upward and bouncing off the top of a window.\n"
      + "Can you add a wind force that changes over time, perhaps according to Perlin noise?";
  }

  public class Balloon : SimpleMover
  {
    private float tRope;
    private float tNoise;
    private OpenSimplexNoise noise;

    public Balloon() : base(WrapModeEnum.Bounce) { }

    public override void _Ready()
    {
      base._Ready();

      var halfSize = GetViewport().Size / 2;
      MaxVelocity = 0.35f;

      noise = new OpenSimplexNoise();
      Position = new Vector2((float)GD.RandRange(halfSize.x / 2, halfSize.x + halfSize.x / 2), (float)GD.RandRange(halfSize.y / 2, halfSize.y + halfSize.y / 2));
      tNoise = (float)GD.RandRange(0, 1000);
      tRope = 0;
    }

    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, BodySize, Colors.White.WithAlpha(128));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.LightCyan.WithAlpha(128));
      DrawLine(Vector2.Down * BodySize, (Vector2.Down * (BodySize * 2)).Rotated(Mathf.Sin(tRope) / 10), Colors.White.WithAlpha(128), 2);
    }

    protected override void UpdateAcceleration()
    {
      var windForce = new Vector2(noise.GetNoise1d(tNoise) * 0.1f, -0.05f);
      ApplyForce(windForce * 0.025f);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      tRope += delta * 10;
      tNoise += delta * 10;
    }
  }

  public override void _Ready()
  {
    int balloonCount = 20;
    foreach (int x in Enumerable.Range(0, balloonCount))
    {
      AddChild(new Balloon());
    }
  }
}
