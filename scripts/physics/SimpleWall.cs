using Godot;

namespace Physics
{
  public class SimpleWall : StaticBody2D
  {
    public Color BaseColor = Colors.Olive;
    public Vector2 MeshSize
    {
      get => bodySize;
      set
      {
        bodySize = value;
        if (rectangleShape2D != null)
        {
          rectangleShape2D.Extents = bodySize;
        }
      }
    }

    private Vector2 bodySize = new Vector2(10, 10);
    private CollisionShape2D collisionShape2D;
    private RectangleShape2D rectangleShape2D;

    public override void _Ready()
    {
      rectangleShape2D = new RectangleShape2D();
      rectangleShape2D.Extents = bodySize / 2;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = rectangleShape2D;
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(-bodySize / 2, bodySize), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
