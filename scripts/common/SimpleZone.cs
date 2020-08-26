using Godot;

public class SimpleZone : Area2D
{
  public Vector2 Size = new Vector2(100, 100);

  protected Font defaultFont;

  public override void _Ready()
  {
    defaultFont = Utils.LoadDefaultFont();
    CreateCollisionShape();
  }

  protected void CreateCollisionShape()
  {
    var collisionShape = new CollisionShape2D();
    var shape = new RectangleShape2D();
    shape.Extents = Size / 2;
    collisionShape.Shape = shape;
    AddChild(collisionShape);
  }

  public override void _Draw()
  {
    DrawZone(Colors.DarkGreen);
  }

  protected void DrawZone(Color color)
  {
    DrawRect(new Rect2(Vector2.Zero - Size / 2, Size), color.WithAlpha(200));
  }
}
