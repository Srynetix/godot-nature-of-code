using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  public class VerletClothBuilder
  {
    public enum PinModeEnum
    {
      None,
      TopCorners,
      Top,
      AllCorners
    }

    private PinModeEnum pinMode;
    private bool drawPoints;
    private VerletWorld world;
    private List<VerletPoint> points;
    private Vector2 pointCount;
    private float pointRadius;

    public VerletClothBuilder(VerletWorld world, PinModeEnum pinMode = PinModeEnum.TopCorners, bool drawPoints = false, float pointRadius = 10f)
    {
      this.world = world;
      this.pinMode = pinMode;
      this.drawPoints = drawPoints;
      this.pointRadius = pointRadius;
      points = new List<VerletPoint>();
    }

    public VerletClothBuilder AddPointsInRectangle(Vector2 topLeftPosition, Vector2 pointCount, Vector2 separation)
    {
      this.pointCount = pointCount;
      points.Clear();

      for (int j = 0; j < pointCount.y; ++j)
      {
        for (int i = 0; i < pointCount.x; ++i)
        {
          var position = topLeftPosition + new Vector2(separation.x * i, separation.y * j);
          var point = world.CreatePoint();
          point.Radius = pointRadius;
          point.Drawing = drawPoints;
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

    public void Build(float restingDistance = 50, float tearSensitivity = 100, float stiffness = 1)
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
            link.RestingDistance = restingDistance;
            link.TearSensitivity = tearSensitivity;
            link.Stiffness = stiffness;
          }

          if (j > 0)
          {
            // Bottom to top
            int pAIdx = i + (j - 1) * (int)pointCount.x;
            int pBIdx = i + j * (int)pointCount.x;

            var link = world.CreateLink(points[pAIdx], points[pBIdx]);
            link.RestingDistance = restingDistance;
            link.TearSensitivity = tearSensitivity;
            link.Stiffness = stiffness;
          }
        }
      }
    }
  }
}
