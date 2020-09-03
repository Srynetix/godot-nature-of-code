using Godot;

namespace Agents
{
  public class SimplePath : Node2D
  {
    public Vector2 Start;
    public Vector2 End;
    public float Radius = 20;

    public override void _Draw()
    {
      DrawLine(Start, End, Colors.LightCyan.WithAlpha(64), Radius * 2);
      DrawLine(Start, End, Colors.Black, 1);
    }
  }
}
