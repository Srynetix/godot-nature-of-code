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
    public Vector2 MeshSize
    {
      get => rectangleShape2D.Extents;
      set
      {
        rectangleShape2D.Extents = bodySize;
      }
    }

    private Vector2 bodySize = new Vector2(20, 20);
    private CollisionShape2D collisionShape2D;
    private RectangleShape2D rectangleShape2D;

    /// <summary>
    /// Create a simple box.
    /// </summary>
    public SimpleBox()
    {
      rectangleShape2D = new RectangleShape2D();
      rectangleShape2D.Extents = new Vector2(10, 10);
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = rectangleShape2D;
    }

    #region Lifecycle methods

    public override void _Ready()
    {
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
      DrawRect(new Rect2(-bodySize / 2, bodySize), OutlineColor);
      DrawRect(new Rect2(-bodySize / 2 + outlineVec / 2, bodySize - outlineVec / 2), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }

    #endregion
  }
}
