using Godot;
using System.Linq;

public class C2Example5 : Node2D, IExample {
  public string _Summary() {
    return "Example 2.5:\n"
      + "Fluid Resistance";
  }

  public class Liquid : Area2D {
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
      Color color = Colors.DarkViolet;

      DrawRect(new Rect2(Vector2.Zero - Size / 2, Size), color.WithAlpha(200));
      var strToDraw = "Liquid: " + Coeff.ToString();
      var strSize = defaultFont.GetStringSize(strToDraw);

      DrawString(defaultFont, Vector2.Left * strSize / 2, strToDraw);
    }

    public override void _Process(float delta) {
      foreach (var area in GetOverlappingAreas()) {
        var mover = (Mover)area;
        mover.ApplyDrag(Coeff);
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

    public void ApplyDrag(float coef) {
      float speedSqr = Velocity.LengthSquared();
      float mag = coef * speedSqr;

      var drag = Velocity.Normalized() * mag * -1;
      ApplyForce(drag);
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
      var gravity = new Vector2(0, 0.098f * Mass);

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

    var zone = new Liquid();
    zone.Coeff = 0.25f;
    zone.Size = new Vector2(size.x, size.y / 4);
    zone.Position = new Vector2(size.x / 2, size.y - size.y / 8);
    AddChild(zone);

    foreach (var x in Enumerable.Range(0, 20)) {
      var mover = new Mover();
      mover.BodySize = (float)GD.RandRange(5, 20);
      mover.Mass = (float)GD.RandRange(5, 10);
      AddChild(mover);
    }
  }
}
