using Godot;
using System.Linq;

public class C2Example6 : Node2D, IExample {
  public string _Summary() {
    return "Example 2.6:\n"
      + "Attraction";
  }

  public class Attractor : Node2D {
    public float Radius = 20.0f;
    public float Mass = 20.0f;
    public float Gravitation = 1.0f;

    public Vector2 Attract(Mover mover) {
      var force = Position - mover.Position;
      var length = Mathf.Clamp(force.Length(), 5, 25);
      float strength = (Gravitation * Mass * mover.Mass) / (length * length);
      return force.Normalized() * strength;
    }

    public override void _Ready() {
      AddToGroup("attractors");
      Position = GetViewport().Size / 2;
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, Radius, Colors.LightGoldenrod);
    }
  }

  public class Mover : Node2D {
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float MaxVelocity = 10.0f;
    public float BodySize = 20;
    public float Mass = 10;

    public void ApplyForce(Vector2 force) {
      Acceleration += force / Mass;
    }

    public void Move() {
      Velocity = (Velocity + Acceleration).Clamped(MaxVelocity);
      Position += Velocity;
      Acceleration = Vector2.Zero;

      BounceOnEdges();
    }

    public void BounceOnEdges() {
      var size = GetViewport().Size;
      var newPos = Position;

      if (Position.y < BodySize / 2) {
        Velocity.y *= -1;
        newPos.y = BodySize / 2;
      }
      else if (Position.y > size.y - BodySize / 2) {
        Velocity.y *= -1;
        newPos.y = size.y - BodySize / 2;
      }

      if (Position.x < BodySize / 2) {
        Velocity.x *= -1;
        newPos.x = BodySize / 2;
      }
      else if (Position.x > size.x - BodySize / 2) {
        Velocity.x *= -1;
        newPos.x = size.x - BodySize / 2;
      }

      Position = newPos;
    }

    public override void _Ready() {
      var size = GetViewport().Size;
      var xPos = (float)GD.RandRange(BodySize, size.x - BodySize);
      var yPos = (float)GD.RandRange(BodySize, size.y - BodySize);
      Position = new Vector2(xPos, yPos);
    }

    public override void _Process(float delta) {
      // For each attractor
      foreach (var n in GetTree().GetNodesInGroup("attractors")) {
        var attractor = (Attractor)n;
        var force = attractor.Attract(this);
        ApplyForce(force);
      }

      Move();
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, BodySize, Colors.LightBlue.WithAlpha(200));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.White.WithAlpha(200));
    }
  }

  public override void _Ready() {
    var size = GetViewport().Size;

    AddChild(new Attractor());
    AddChild(new Mover());
  }
}
