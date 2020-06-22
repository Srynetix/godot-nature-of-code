using Godot;

/**
Example 1.11:
Array of movers accelerating towards the mouse
*/

public class C1Example11 : Node2D, IExample {
  public class Mover : Node2D {
    private Vector2 velocity;
    private Vector2 acceleration;
    private float topSpeed;

    public override void _Ready() {
      var size = GetViewport().Size;
      Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
      velocity = Vector2.Zero;
      acceleration = Vector2.Zero;
      topSpeed = 10;
    }

    public override void _Process(float delta) {
      var mousePos = GetViewport().GetMousePosition();
      var dir = (mousePos - Position).Normalized();

      acceleration = dir * 0.5f;

      velocity += acceleration;
      velocity = velocity.Clamped(topSpeed);
      Position += velocity;
      WrapEdges();
    }

    private void WrapEdges() {
      var size = GetViewport().Size;

      if (Position.x > size.x) {
        Position = new Vector2(0, Position.y);
      }
      else if (Position.x < 0) {
        Position = new Vector2(size.x, Position.y);
      }

      if (Position.y > size.y) {
        Position = new Vector2(Position.x, 0);
      }
      else if (Position.y < 0) {
        Position = new Vector2(Position.x, size.y);
      }
    }

    public override void _Draw() {
      DrawCircle(Vector2.Zero, 20, Color.Color8(64, 64, 64, 64));
    }
  }

  public string _Summary() {
    return "Example 1.11:\nArray of movers accelerating towards the mouse";
  }

  public override void _Ready() {
    GD.Randomize();

    var moverCount = 20;
    for (int i = 0; i < moverCount; ++i) {
      var mover = new Mover();
      AddChild(mover);
    }
  }
}
