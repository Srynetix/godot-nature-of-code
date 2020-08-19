using Godot;
using System.Collections.Generic;

public class SimpleStaticChain : StaticBody2D
{
  public Color BaseColor = Colors.Olive;

  private List<CollisionShape2D> shapes;
  private List<SegmentShape2D> segments;

  public SimpleStaticChain()
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
      DrawLine(segment.A, segment.B, BaseColor, 1);
    }
  }

  public override void _Process(float delta)
  {
    Update();
  }
}
