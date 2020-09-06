using Godot;

namespace Physics
{
  /// <summary>
  /// Simple physics polygon.
  /// </summary>
  public class SimplePolygon : RigidBody2D
  {
    /// <summary>Polygon points</summary>
    public Vector2[] Points;

    /// <summary>Color</summary>
    public Color BaseColor;

    private readonly CollisionShape2D collisionShape2D;
    private readonly ConvexPolygonShape2D polygonShape2D;
    private Color[] colors;

    /// <summary>
    /// Create a default polygon.
    /// </summary>
    public SimplePolygon()
    {
      polygonShape2D = new ConvexPolygonShape2D();
      collisionShape2D = new CollisionShape2D { Shape = polygonShape2D };
    }

    public override void _Ready()
    {
      polygonShape2D.SetPointCloud(Points);
      AddChild(collisionShape2D);

      colors = new Color[polygonShape2D.Points.Length];
      for (int i = 0; i < polygonShape2D.Points.Length; ++i)
      {
        colors[i] = BaseColor;
      }
    }

    public override void _Draw()
    {
      DrawPolygon(polygonShape2D.Points, colors);
    }
  }
}
