using Godot;
using System.Linq;

public class C2Exercise3 : Node2D, IExample {
  public string _Summary() {
    return "Exercise 2.3:\n"
      + "Instead of objects bouncing off the edge of the wall, create an example in which an invisible force pushes back on the objects to keep them in the window.\n"
      + "Can you weight the force according to how far the object is from an edge—i.e., the closer it is, the stronger the force?";
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

    public Vector2 ComputeWindForce() {
      var size = GetViewport().Size;
      var pos = Position;
      var output = Vector2.Zero;
      var limit = BodySize * 8;

      // Push left
      if (Position.x > size.x - limit) {
        var force = limit * 2 - (size.x - Position.x);
        output.x = -force * 0.01f;
      }

      // Push right
      else if (Position.x < limit) {
        var force = limit * 2 - Position.x;
        output.x = force * 0.01f;
      }

      else {
        output.x = 0.1f;
      }

      return output;
    }

    public override void _Ready() {
      var size = GetViewport().Size;
      var xPos = (float)GD.RandRange(BodySize * 4, size.x - BodySize * 4);
      Position = new Vector2(xPos, size.y / 2);
    }

    public override void _Process(float delta) {
      var gravity = new Vector2(0, 0.9f);

      ApplyForce(ComputeWindForce());
      ApplyForce(gravity);

      Move();
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, BodySize, Colors.LightBlue.WithAlpha(200));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.White.WithAlpha(200));
    }
  }

  public override void _Ready() {
    foreach (var x in Enumerable.Range(0, 20)) {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = (float)GD.RandRange(5, 10);
      AddChild(mover);
    }
  }
}
