using Godot;
using System.Collections.Generic;

namespace Physics
{
  public class SimpleStaticLines : StaticBody2D
  {
    public Color BaseColor = Colors.Olive;

    private List<CollisionShape2D> shapes;
    private List<SegmentShape2D> segments;

    public SimpleStaticLines()
    {
      shapes = new List<CollisionShape2D>();
      segments = new List<SegmentShape2D>();
    }

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
    }

    public override void _Draw()
    {
      foreach (var segment in segments)
      {
        DrawLine(segment.A, segment.B, BaseColor, 2);
      }
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
