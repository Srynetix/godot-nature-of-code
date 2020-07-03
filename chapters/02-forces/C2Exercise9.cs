using Godot;
using System.Linq;

public class C2Exercise9 : Node2D, IExample {
  public string _Summary() {
    return "Exercise 2.9:\n"
      + "What happens if you design a force that is weaker the closer it gets and stronger the farther it gets?\n"
      + "Or what if you design your attractor to attract faraway objects, but repel close ones?";
  }

  public class Trail : Line2D {
    public int PointCount = 50;
    public Node2D Target;

    public override void _Ready() {
      Width = 2.5f;

      // Gradient
      var gradient = new Gradient();
      gradient.SetColor(0, Colors.Purple.WithAlpha(0));
      gradient.AddPoint(1, Colors.Purple);
      gradient.SetOffset(0, 0);
      gradient.SetOffset(1, 1);

      Gradient = gradient;
    }

    public override void _Process(float delta) {
      AddPoint(Target.GlobalPosition);
      while (GetPointCount() > PointCount) {
        RemovePoint(0);
      }
    }
  }

  public class Attractor : Node2D {
    public float Radius = 20.0f;
    public float Mass = 20.0f;
    public float Gravitation = 1.0f;

    public Vector2 Attract(Mover mover) {
      var force = Position - mover.Position;
      var length = Mathf.Clamp(force.Length(), 5, 50);
      var coef = 1;

      // Push if too close
      if (length < 45) {
        coef = -3;
      }

      float strength = (Gravitation * Mass * mover.Mass) / (length * length);
      return force.Normalized() * strength * coef;
    }

    public override void _Ready() {
      AddToGroup("attractors");
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, Radius, Colors.LightGoldenrod.WithAlpha(10));
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

    var attractor1 = new Attractor();
    attractor1.Position = new Vector2(size.x / 4, size.y / 2);
    AddChild(attractor1);

    var attractor2 = new Attractor();
    attractor2.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(attractor2);

    var attractor3 = new Attractor();
    attractor3.Position = new Vector2(size.x - size.x / 4, size.y / 2);
    AddChild(attractor3);

    foreach (var x in Enumerable.Range(0, 10)) {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = mover.BodySize;

      var trail = new Trail();
      trail.Target = mover;

      AddChild(trail);
      AddChild(mover);
    }
  }
}
