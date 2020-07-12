using System.Linq;

using Godot;

public class C2Exercise1 : Node2D, IExample {
  public string _Summary() {
    return "Exercise 2.1:\n"
      + "Using forces, simulate a helium-filled balloon floating upward and bouncing off the top of a window.\n"
      + "Can you add a wind force that changes over time, perhaps according to Perlin noise?";
  }

  public class Mover : Node2D {
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float MaxVelocity = 1.0f;
    public float BodySize = 20;

    public void ApplyForce(Vector2 force) {
      Acceleration = force;
    }

    public void Move() {
      Velocity = (Velocity + Acceleration).Clamped(MaxVelocity);
      Position += Velocity;
      Acceleration *= 0;

      BounceOnEdges();
    }

    public void BounceOnEdges() {
      var size = GetViewport().Size;

      if (Position.y < BodySize / 2 || Position.y > size.y - BodySize / 2) {
        Velocity.y *= -1;
      }

      if (Position.x < BodySize / 2 || Position.x > size.x - BodySize / 2) {
        Velocity.x *= -1;
      }
    }
  }

  public class Balloon : Mover {
    private float tRope;
    private float tNoise;
    private OpenSimplexNoise noise;

    public override void _Ready() {
      var halfSize = GetViewport().Size / 2;

      noise = new OpenSimplexNoise();
      Position = new Vector2((float)GD.RandRange(halfSize.x / 2, halfSize.x + halfSize.x / 2), (float)GD.RandRange(halfSize.y / 2, halfSize.y + halfSize.y / 2));
      tNoise = (float)GD.RandRange(0, 1000);
      tRope = 0;
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, BodySize, Colors.White.WithAlpha(128));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.LightCyan.WithAlpha(128));
      DrawLine(Vector2.Down * BodySize, (Vector2.Down * (BodySize * 2)).Rotated(Mathf.Sin(tRope) / 10), Colors.White.WithAlpha(128), 2);
    }

    public override void _Process(float delta) {
      var windForce = new Vector2(noise.GetNoise1d(tNoise), (float)GD.RandRange(-0.75f, 0.25f));
      ApplyForce(windForce * 2.0f * delta);
      Move();

      Update();
      tRope += delta * 10;
      tNoise += delta * 10;
    }
  }

  public override void _Ready() {
    int balloonCount = 20;
    foreach (int x in Enumerable.Range(0, balloonCount)) {
      AddChild(new Balloon());
    }
  }
}
