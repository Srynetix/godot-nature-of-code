using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  /// <summary>
  /// Simple verlet cluster.
  /// </summary>
  public class VerletCluster
  {
    /// <summary>Cluster points</summary>
    public List<VerletPoint> Points;

    /// <summary>
    /// Create a verlet cluster.
    /// </summary>
    /// <param name="world">Verlet world</param>
    /// <param name="centerPosition">Center position</param>
    /// <param name="pointCount">Point count</param>
    /// <param name="diameter">Cluster diameter</param>
    /// <param name="gravityScale">Gravity scale</param>
    /// <param name="tearSensitivityFactor">Distance factor required to break the cloth. Use `-1` to create an unbreakable cloth.</param>
    /// <param name="stiffness">Stiffness of the cloth</param>
    /// <param name="drawPoints">Draw points</param>
    /// <param name="pointRadius">Verlet point radius</param>
    public VerletCluster(VerletWorld world, Vector2 centerPosition, int pointCount, float diameter, float gravityScale = 1, float tearSensitivityFactor = -1, float stiffness = 0.1f, bool drawPoints = true, float pointRadius = 10f)
    {
      Points = new List<VerletPoint>();

      for (int i = 0; i < pointCount; ++i)
      {
        var position = centerPosition + new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1));
        var point = world.CreatePoint();
        point.GravityScale = gravityScale;
        point.Radius = pointRadius;
        point.Visible = drawPoints;
        point.MoveToPosition(position);
        Points.Add(point);
      }

      for (int i = 0; i < Points.Count - 1; ++i)
      {
        for (int j = i + 1; j < Points.Count; ++j)
        {
          var a = Points[i];
          var b = Points[j];

          var link = world.CreateLink(a, b);
          link.RestingDistance = diameter;
          link.TearSensitivity = diameter * tearSensitivityFactor;
          link.Stiffness = stiffness;
        }
      }
    }
  }
}
