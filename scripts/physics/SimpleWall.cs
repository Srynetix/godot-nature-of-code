using Godot;

namespace Physics
{
  /// <summary>
  /// Simple static wall.
  /// </summary>
  public class SimpleWall : StaticBody2D
  {
    /// <summary>Wall color</summary>
    public Color BaseColor = Colors.Olive;

    /// <summary>Wall size</summary>
    public Vector2 BodySize
    {
      get => rectangleShape2D.Extents * 2;
      set => rectangleShape2D.Extents = value / 2;
    }

    private readonly CollisionShape2D collisionShape2D;
    private readonly RectangleShape2D rectangleShape2D;

    /// <summary>
    /// Create a default static wall with 10x10px extents.
    /// </summary>
    public SimpleWall()
    {
      collisionShape2D = new CollisionShape2D { Shape = rectangleShape2D };
      collisionShape2D.Shape = rectangleShape2D;
    }

    public override void _Ready()
    {
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(-BodySize / 2, BodySize), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
