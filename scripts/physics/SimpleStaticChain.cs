using Godot;
using System.Collections.Generic;
using Drawing;

namespace Physics
{
  /// <summary>
  /// Simple static lines.
  /// Can be used to draw physics outlines.
  /// </summary>
  public class SimpleStaticLines : StaticBody2D
  {
    /// <summary>Color</summary>
    public Color BaseColor = Colors.LightGoldenrod;

    private List<CollisionShape2D> shapes;
    private List<SegmentShape2D> segments;
    private List<SimpleLineSprite> lineSprites;

    /// <summary>
    /// Create empty outline.
    /// </summary>
    public SimpleStaticLines()
    {
      shapes = new List<CollisionShape2D>();
      segments = new List<SegmentShape2D>();
      lineSprites = new List<SimpleLineSprite>();
    }

    /// <summary>
    /// Add a line segment.
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="end">End position</param>
    public void AddSegment(Vector2 start, Vector2 end)
    {
      var shape = new CollisionShape2D();
      var segment = new SegmentShape2D();
      segment.A = start;
      segment.B = end;
      shape.Shape = segment;

      segments.Add(segment);
      shapes.Add(shape);
      AddChild(shape);

      var lineSprite = new SimpleLineSprite();
      lineSprite.PositionA = GlobalPosition + segment.A;
      lineSprite.PositionB = GlobalPosition + segment.B;
      lineSprite.Modulate = BaseColor;
      lineSprite.Width = 2;
      lineSprites.Add(lineSprite);
      AddChild(lineSprite);
    }
  }
}
