using Godot;

namespace Physics
{
  public class SimpleBall : RigidBody2D
  {
    public Color BaseColor
    {
      get => baseColor;
      set
      {
        baseColor = value;
        if (sprite != null)
        {
          sprite.BaseColor = value;
        }
      }
    }
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

    private Color baseColor = Colors.White;
    private float radius = 10;
    private CollisionShape2D collisionShape2D;
    private CircleShape2D circleShape2D;
    private SimpleCircleSprite sprite;

    public override void _Ready()
    {
      Mass = 0.25f;
      circleShape2D = new CircleShape2D();
      circleShape2D.Radius = radius;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = circleShape2D;
      AddChild(collisionShape2D);

      sprite = new SimpleCircleSprite();
      sprite.Radius = Radius;
      sprite.BaseColor = BaseColor;
      AddChild(sprite);
    }
  }
}
