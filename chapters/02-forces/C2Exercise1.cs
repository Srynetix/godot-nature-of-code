using System.Linq;
using Godot;
using Drawing;
using Forces;

public class C2Exercise1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.1:\n"
      + "Helium-filled balloons";
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

      var halfSize = GetViewportRect().Size / 2;
      MaxVelocity = 0.35f;

      noise = new OpenSimplexNoise();
      Position = new Vector2((float)GD.RandRange(halfSize.x / 2, halfSize.x + halfSize.x / 2), (float)GD.RandRange(halfSize.y / 2, halfSize.y + halfSize.y / 2));
      tNoise = (float)GD.RandRange(0, 1000);
      tRope = 0;

      // Set draw method
      Mesh.MeshType = SimpleMesh.TypeEnum.Custom;
      Mesh.CustomDrawMethod = (pen) =>
      {
        pen.DrawCircle(Vector2.Zero, Radius, pen.Modulate.WithAlpha(128));
        pen.DrawLine(Vector2.Down * Radius, (Vector2.Down * (pen.MeshSize * 2)).Rotated(Mathf.Sin(tRope) / 10), Colors.White.WithAlpha(128), 2);
      };
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
