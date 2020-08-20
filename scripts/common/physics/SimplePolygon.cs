using Godot;

public class SimplePolygon : RigidBody2D
{
  public Vector2[] Points;
  public Color BaseColor;

  private CollisionShape2D collisionShape2D;
  private ConvexPolygonShape2D polygonShape2D;
  private Color[] colors;

  public SimplePolygon()
  {
  }

  public override void _Ready()
  {
    polygonShape2D = new ConvexPolygonShape2D();
    polygonShape2D.SetPointCloud(Points);
    collisionShape2D = new CollisionShape2D();
    collisionShape2D.Shape = polygonShape2D;
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
