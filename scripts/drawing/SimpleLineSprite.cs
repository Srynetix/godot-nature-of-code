using Godot;
using Assets;

namespace Drawing
{
  /// <summary>
  /// Use this to draw a line between two points.
  /// Instead of DrawLine and Line2D, it can be batched.
  /// </summary>
  public class SimpleLineSprite : Sprite
  {
    /// <summary>
    /// Point Position A.
    /// </summary>
    public Vector2 PositionA
    {
      get => positionA;
      set
      {
        positionA = value;
        UpdateTransform();
      }
    }

    /// <summary>
    /// Point Position B.
    /// </summary>
    public Vector2 PositionB
    {
      get => positionB;
      set
      {
        positionB = value;
        UpdateTransform();
      }
    }

    /// <summary>
    /// Line Width.
    /// </summary>
    public float Width
    {
      get => width;
      set
      {
        width = value;
        Scale = new Vector2(width, Scale.y);
      }
    }

    private Vector2 positionA = Vector2.Zero;
    private Vector2 positionB = Vector2.Zero;
    private float width = 1;

    /// <summary>
    /// Create a grey line sprite.
    /// </summary>
    public SimpleLineSprite()
    {
      Modulate = Colors.LightGray;
    }

    public override void _Ready()
    {
      Texture = SimpleDefaultTexture.LineTexture;
      UpdateTransform();
    }

    private void UpdateTransform()
    {
      var dir = positionB - positionA;
      Rotation = dir.Angle() + (Mathf.Pi / 2);
      if (Texture != null)
      {
        // Adapt scale depending on vector length
        Scale = new Vector2(Scale.x, dir.Length() / Texture.GetSize().y);
      }

      // Fix position
      GlobalPosition = positionA + (dir / 2);
    }
  }
}
