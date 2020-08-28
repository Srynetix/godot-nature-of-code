using Godot;
using Assets;

namespace Forces
{
  /// <summary>
  /// Simple collision zone.
  /// </summary>
  public class SimpleZone : Area2D
  {
    /// <summary>Zone size</summary>
    public Vector2 Size = new Vector2(100, 100);

    protected Font defaultFont;

    /// <summary>
    /// Draw zone.
    /// </summary>
    /// <param name="color">Color</param>
    protected void DrawZone(Color color)
    {
      DrawRect(new Rect2(Vector2.Zero - Size / 2, Size), color.WithAlpha(200));
    }

    #region Lifecycle methods

    public override void _Ready()
    {
      defaultFont = SimpleDefaultFont.LoadDefaultFont();
      CreateCollisionShape();
    }

    public override void _Draw()
    {
      DrawZone(Colors.DarkGreen);
    }

    #endregion

    #region Private methods

    private void CreateCollisionShape()
    {
      var collisionShape = new CollisionShape2D();
      var shape = new RectangleShape2D();
      shape.Extents = Size / 2;
      collisionShape.Shape = shape;
      AddChild(collisionShape);
    }

    #endregion
  }
}
