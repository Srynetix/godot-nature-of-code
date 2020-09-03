using Godot;
using System.Collections.Generic;

namespace Agents
{
  /// <summary>
  /// Simple path for SimpleVehicle to follow.
  /// </summary>
  public class SimplePath : Node2D
  {
    /// <summary>Points</summary>
    public List<Vector2> Points;
    /// <summary>Path radius</summary>
    public float Radius = 20;

    public SimplePath()
    {
      Points = new List<Vector2>();
    }

    public override void _Draw()
    {
      if (Points.Count < 2)
      {
        GD.PrintErr("SimplePath should contain at least 2 points");
        return;
      }

      for (int i = 0; i < Points.Count - 1; ++i)
      {
        var p1 = Points[i];
        var p2 = Points[i + 1];

        DrawCircle(p1, Radius, Colors.DarkGoldenrod);
        DrawLine(p1, p2, Colors.DarkGoldenrod, Radius * 2);
        DrawCircle(p2, Radius, Colors.DarkGoldenrod);
      }

      for (int i = 0; i < Points.Count - 1; ++i)
      {
        var p1 = Points[i];
        var p2 = Points[i + 1];
        DrawLine(p1, p2, Colors.Black, 1);
      }
    }
  }
}
