using Godot;
using System.Linq;

public class C2Exercise4 : Node2D, IExample {
  public string _Summary() {
    return "Exercise 2.4:\n"
      + "Create pockets of friction so that objects only experience friction when crossing over those pockets.\n"
      + "What if you vary the strength (friction coefficient) of each area?\n"
      + "What if you make some pockets feature the opposite of friction—i.e., when you enter a given pocket you actually speed up instead of slowing down?";
  }

  public class Pocket : Area2D {
    public Vector2 Size = new Vector2(100, 100);
    public float Coeff = 0;

    private Font defaultFont;

    public override void _Ready() {
      SetCollisionLayerBit(0, false);
      SetCollisionLayerBit(1, true);
      SetCollisionMaskBit(0, true);
      SetCollisionMaskBit(1, true);

      // Add CollisionShape
      var collisionShape = new CollisionShape2D();
      var shape = new RectangleShape2D();
      shape.Extents = Size / 2;
      collisionShape.Shape = shape;
      AddChild(collisionShape);

      defaultFont = Utils.LoadDefaultFont();
    }

    public override void _Draw() {
      Color color;
      if (Coeff > 0) {
        color = Colors.DarkRed;
      }
      else {
        color = Colors.LightBlue;
      }

      DrawRect(new Rect2(Vector2.Zero - Size / 2, Size), color.WithAlpha(200));
      var strToDraw = "Friction: " + Coeff.ToString();
      var strSize = defaultFont.GetStringSize(strToDraw);

      DrawString(defaultFont, Vector2.Left * strSize / 2, strToDraw);
    }

    public override void _Process(float delta) {
      foreach (var area in GetOverlappingAreas()) {
        var mover = (Mover)area;
        mover.ApplyFriction(Coeff);
      }
    }
  }

  public class Mover : Area2D {
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float MaxVelocity = 10.0f;
    public float BodySize = 20;
    public float Mass = 10;

    public void ApplyForce(Vector2 force) {
      Acceleration += force / Mass;
    }

    public void ApplyFriction(float coef) {
      var friction = Velocity.Normalized() * -coef;
      ApplyForce(friction);
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
      SetCollisionLayerBit(0, true);
      SetCollisionMaskBit(0, true);
      SetCollisionMaskBit(1, true);

      var size = GetViewport().Size;
      var xPos = (float)GD.RandRange(BodySize, size.x - BodySize);
      Position = new Vector2(xPos, size.y / 2);

      // Add CollisionShape
      var collisionShape = new CollisionShape2D();
      var shape = new CircleShape2D();
      shape.Radius = BodySize;
      collisionShape.Shape = shape;
      AddChild(collisionShape);
    }

    public override void _Process(float delta) {
      var wind = new Vector2(0.1f, 0);
      var gravity = new Vector2(0, 0.98f);

      ApplyForce(wind);
      ApplyForce(gravity);

      Move();
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, BodySize, Colors.LightBlue.WithAlpha(200));
      DrawCircle(Vector2.Zero, BodySize - 2, Colors.White.WithAlpha(200));
    }
  }

  public override void _Ready() {
    var size = GetViewport().Size;

    var zone1 = new Pocket();
    zone1.Coeff = 0.25f;
    zone1.Size = new Vector2(100, size.y);
    zone1.Position = new Vector2(size.x / 4, size.y / 2);
    AddChild(zone1);

    var zone2 = new Pocket();
    zone2.Coeff = -0.25f;
    zone2.Size = new Vector2(100, size.y);
    zone2.Position = new Vector2(size.x / 2 + size.x / 4, size.y / 2);
    AddChild(zone2);

    var zone3 = new Pocket();
    zone3.Coeff = -2f;
    zone3.Size = new Vector2(10, size.y);
    zone3.Position = new Vector2(size.x / 2, size.y / 2);
    AddChild(zone3);

    foreach (var x in Enumerable.Range(0, 20)) {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = (float)GD.RandRange(5, 10);
      AddChild(mover);
    }
  }
}
