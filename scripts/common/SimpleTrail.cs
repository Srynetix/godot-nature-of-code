using Godot;

public class SimpleTrail : Line2D
{
  public int PointCount = 50;
  public Color TrailColor = Colors.Purple;
  public Node2D Target;

  public SimpleTrail()
  {
    Width = 2.5f;
  }

  public override void _Ready()
  {
    // Gradient
    var gradient = new Gradient();
    gradient.SetColor(0, TrailColor.WithAlpha(0));
    gradient.AddPoint(1, TrailColor);
    gradient.SetOffset(0, 0);
    gradient.SetOffset(1, 1);

    Gradient = gradient;
  }

  public override void _Process(float delta)
  {
    AddPoint(Target.GlobalPosition);
    while (GetPointCount() > PointCount)
    {
      RemovePoint(0);
    }
  }
}