using Godot;
using Drawing;

/// <summary>
/// Physics-related primitives.
/// </summary>
namespace Physics
{
  /// <summary>
  /// Simple physics ball.
  /// </summary>
  public class SimpleBall : RigidBody2D
  {
    /// <summary>Base color</summary>
    public Color BaseColor
    {
      get => sprite.Modulate;
      set
      {
        sprite.Modulate = value;
      }
    }

    /// <summary>Ball radius</summary>
    public float Radius
    {
      get => circleShape2D.Radius;
      set
      {
        sprite.Radius = value;
        circleShape2D.Radius = value;
      }
    }

    private CollisionShape2D collisionShape2D;
    private CircleShape2D circleShape2D;
    private SimpleCircleSprite sprite;

    /// <summary>
    /// Create a default ball.
    /// </summary>
    public SimpleBall()
    {
      Mass = 0.25f;
      circleShape2D = new CircleShape2D();
      circleShape2D.Radius = 10;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = circleShape2D;

      sprite = new SimpleCircleSprite();
      sprite.Radius = 10;
      sprite.Modulate = BaseColor;
    }

    #region Lifecycle methods

    public override void _Ready()
    {
      AddChild(collisionShape2D);
      AddChild(sprite);
    }

    #endregion
  }
}
