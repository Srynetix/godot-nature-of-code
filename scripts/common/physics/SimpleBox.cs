using Godot;

namespace Physics
{
  public class SimpleBox : RigidBody2D
  {
    public float OutlineWidth = 2;
    public Color OutlineColor = Colors.LightBlue;
    public Color BaseColor = Colors.White;
    public Vector2 BodySize
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

    private Vector2 bodySize = new Vector2(20, 20);
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
      var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
      DrawRect(new Rect2(-bodySize / 2, bodySize), OutlineColor);
      DrawRect(new Rect2(-bodySize / 2 + outlineVec / 2, bodySize - outlineVec / 2), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
