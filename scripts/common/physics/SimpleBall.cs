using Godot;

public class SimpleBall : RigidBody2D
{
  public float OutlineWidth = 2;
  public Color OutlineColor = Colors.LightBlue;
  public Color BaseColor = Colors.White;
  public float Radius
  {
    get => radius;
    set
    {
      radius = value;
      if (circleShape2D != null)
      {
        circleShape2D.Radius = value;
      }
    }
  }

  private float radius = 10;
  private CollisionShape2D collisionShape2D;
  private CircleShape2D circleShape2D;

  public override void _Ready()
  {
    Mass = 0.25f;
    circleShape2D = new CircleShape2D();
    circleShape2D.Radius = radius;
    collisionShape2D = new CollisionShape2D();
    collisionShape2D.Shape = circleShape2D;
    AddChild(collisionShape2D);
  }

  public override void _Draw()
  {
    var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
    DrawCircle(Vector2.Zero, Radius, OutlineColor);
    DrawCircle(Vector2.Zero, Radius - OutlineWidth, BaseColor);
  }

  public override void _Process(float delta)
  {
    Update();
  }
}
