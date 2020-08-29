using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  /// <summary>
  /// Simple verlet cloth builder.
  /// </summary>
  public class VerletClothBuilder
  {
    /// <summary>
    /// Pin mode enum.
    /// </summary>
    public enum PinModeEnum
    {
      /// <summary>No pin</summary>
      None,
      /// <summary>Pin top corners</summary>
      TopCorners,
      /// <summary>Pin all top points</summary>
      Top,
      /// <summary>Pin all corners</summary>
      AllCorners
    }

    private PinModeEnum pinMode;
    private bool drawPoints;
    private VerletWorld world;
    private List<VerletPoint> points;
    private Vector2 pointCount;
    private float separation;
    private float pointRadius;

    public VerletClothBuilder(VerletWorld world, PinModeEnum pinMode = PinModeEnum.TopCorners, bool drawPoints = false, float pointRadius = 10f)
    {
      this.world = world;
      this.pinMode = pinMode;
      this.drawPoints = drawPoints;
      this.pointRadius = pointRadius;
      points = new List<VerletPoint>();
    }

    /// <summary>
    /// Generate points from a top-left position, using X/Y point count and separation to create a verlet points rectangle. 
    /// </summary>
    /// <param name="topLeftPosition">Top-left position</param>
    /// <param name="pointCount">X/Y point count</param>
    /// <param name="separation">Separation</param>
    /// <returns>Builder</returns>
    public VerletClothBuilder GeneratePointsFromPosition(Vector2 topLeftPosition, Vector2 pointCount, float separation)
    {
      this.pointCount = pointCount;
      this.separation = separation;
      points.Clear();

      for (int j = 0; j < pointCount.y; ++j)
      {
        for (int i = 0; i < pointCount.x; ++i)
        {
          var position = topLeftPosition + new Vector2(separation * i, separation * j);
          var point = world.CreatePoint();
          point.Radius = pointRadius;
          point.Visible = drawPoints;
          point.MoveToPosition(position);

          if (pinMode == PinModeEnum.AllCorners)
          {
            if ((j == 0 || j == pointCount.y - 1) && (i == 0 || i == pointCount.x - 1))
            {
              point.PinToCurrentPosition();
            }
          }

          else if (pinMode == PinModeEnum.TopCorners)
          {
            if (j == 0 && (i == 0 || i == pointCount.x - 1))
            {
              point.PinToCurrentPosition();
            }
          }

          else if (pinMode == PinModeEnum.Top)
          {
            if (j == 0)
            {
              point.PinToCurrentPosition();
            }
          }

          points.Add(point);
        }
      }

      return this;
    }

    /// <summary>
    /// Build the verlet cloth.
    /// </summary>
    /// <param name="tearSensitivityFactor">Distance factor required to break the cloth. Use `-1` to create an unbreakable cloth.</param>
    /// <param name="stiffness">Stiffness of the cloth</param>
    public void Build(float tearSensitivityFactor = 2, float stiffness = 1)
    {
      if (points.Count == 0)
      {
        GD.PrintErr("Bad points length for cloth. Need to be > 0");
        return;
      }

      for (int j = 0; j < pointCount.y; ++j)
      {
        for (int i = 0; i < pointCount.x; ++i)
        {
          if (i > 0)
          {
            // Right to left
            int pAIdx = (i - 1) + j * (int)pointCount.x;
            int pBIdx = i + j * (int)pointCount.x;

            var link = world.CreateLink(points[pAIdx], points[pBIdx]);
            link.RestingDistance = separation;
            link.TearSensitivity = separation * tearSensitivityFactor;
            link.Stiffness = stiffness;
          }

          if (j > 0)
          {
            // Bottom to top
            int pAIdx = i + (j - 1) * (int)pointCount.x;
            int pBIdx = i + j * (int)pointCount.x;

            var link = world.CreateLink(points[pAIdx], points[pBIdx]);
            link.RestingDistance = separation;
            link.TearSensitivity = separation * tearSensitivityFactor;
            link.Stiffness = stiffness;
          }
        }
      }
    }
  }
}
