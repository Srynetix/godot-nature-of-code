using Godot;
using System.Collections.Generic;

namespace VerletPhysics {
  /// <summary>
  /// Simple verlet cluster builder.
  /// </summary>
  public class VerletClusterBuilder
  {
    private VerletWorld world;
    private List<VerletPoint> points;
    private float pointRadius;
    private float diameter;
    private bool drawPoints;

    public VerletClusterBuilder(VerletWorld world, bool drawPoints, float pointRadius) {
      this.world = world;
      this.drawPoints = drawPoints;
      this.pointRadius = pointRadius;
      points = new List<VerletPoint>();
    }

    /// <summary>
    /// Generate points from a center position, using point count and diameter to create verlet points. 
    /// </summary>
    /// <param name="centerPosition">Center position</param>
    /// <param name="pointCount">Point count</param>
    /// <param name="diameter">Cluster diameter</param>
    /// <returns>Builder</returns>
    public VerletClusterBuilder GeneratePointsFromPosition(Vector2 centerPosition, int pointCount, float diameter) {
      this.diameter = diameter;

      points.Clear();

      for (int i = 0; i < pointCount; ++i) {
        var position = centerPosition + new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1));
        var point = world.CreatePoint();
        point.Radius = pointRadius;
        point.Visible = drawPoints;
        point.MoveToPosition(position);
        points.Add(point);
      }

      return this;
    }

    /// <summary>
    /// Build the verlet cluster.
    /// </summary>
    /// <param name="tearSensitivity">Distance factor required to break the cloth. Use `-1` to create an unbreakable cloth.</param>
    /// <param name="stiffness">Stiffness of the cloth</param>
    public void Build(float tearSensitivityFactor = 2, float stiffness = 1) {
      for (int i = 0; i < points.Count - 1; ++i) {
        for (int j = i + 1; j < points.Count; ++j) {
          var a = points[i];
          var b = points[j];

          var link = world.CreateLink(a, b);
          link.RestingDistance = diameter;
          link.TearSensitivity = diameter * tearSensitivityFactor;
          link.Stiffness = stiffness;
        }
      }
    }
  }
}
