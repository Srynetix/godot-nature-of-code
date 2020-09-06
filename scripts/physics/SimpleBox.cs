using Godot;

namespace Physics
{
  /// <summary>
  /// Simple physics box.
  /// </summary>
  public class SimpleBox : RigidBody2D
  {
    /// <summary>Outline width</summary>
    public float OutlineWidth = 2;

    /// <summary>Outline color</summary>
    public Color OutlineColor = Colors.LightBlue;

    /// <summary>Base color</summary>
    public Color BaseColor = Colors.White;

    /// <summary>Mesh size</summary>
    public Vector2 BodySize
    {
      get => rectangleShape2D.Extents * 2;
      set
      {
        rectangleShape2D.Extents = value / 2;
      }
    }

    private readonly CollisionShape2D collisionShape2D;
    private readonly RectangleShape2D rectangleShape2D;

    /// <summary>
    /// Create a simple box.
    /// </summary>
    public SimpleBox()
    {
      rectangleShape2D = new RectangleShape2D { Extents = new Vector2(10, 10) };
      collisionShape2D = new CollisionShape2D { Shape = rectangleShape2D };
    }

    public override void _Ready()
    {
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
      DrawRect(new Rect2(-BodySize / 2, BodySize), OutlineColor);
      DrawRect(new Rect2((-BodySize / 2) + (outlineVec / 2), BodySize - (outlineVec / 2)), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
