using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  /// <summary>
  /// Simple verlet cloth.
  /// </summary>
  public class VerletCloth
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

    private readonly List<VerletPoint> points;

    /// <summary>
    /// Create a verlet cloth.
    /// </summary>
    /// <param name="world">Verlet world</param>
    /// <param name="topLeftPosition">Top-left position</param>
    /// <param name="pointCount">X/Y point count</param>
    /// <param name="separation">Separation</param>
    /// <param name="pinMode">Pin mode</param>
    /// <param name="tearSensitivityFactor">Distance factor required to break the cloth. Use `-1` to create an unbreakable cloth.</param>
    /// <param name="stiffness">Stiffness of the cloth</param>
    /// <param name="drawPoints">Draw verlet points</param>
    /// <param name="pointRadius">Verlet point radius</param>
    public VerletCloth(VerletWorld world, Vector2 topLeftPosition, Vector2 pointCount, float separation, PinModeEnum pinMode = PinModeEnum.TopCorners, float tearSensitivityFactor = 2, float stiffness = 1, bool drawPoints = false, float pointRadius = 10f)
    {
      points = new List<VerletPoint>();

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
            int pAIdx = i - 1 + (j * (int)pointCount.x);
            int pBIdx = i + (j * (int)pointCount.x);

            var link = world.CreateLink(points[pAIdx], points[pBIdx]);
            link.RestingDistance = separation;
            link.TearSensitivity = separation * tearSensitivityFactor;
            link.Stiffness = stiffness;
          }

          if (j > 0)
          {
            // Bottom to top
            int pAIdx = i + ((j - 1) * (int)pointCount.x);
            int pBIdx = i + (j * (int)pointCount.x);

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
