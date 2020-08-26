using Godot;
using System.Collections.Generic;

namespace VerletPhysics
{
  public class VerletChainBuilder
  {
    public delegate void PointConfigurator(VerletPoint point);

    private bool pinFirst;
    private bool pinLast;
    private bool drawIntermediatePoints;
    private VerletWorld world;
    private List<VerletPoint> points;

    public VerletChainBuilder(VerletWorld world, bool pinFirst = true, bool pinLast = false, bool drawIntermediatePoints = false)
    {
      this.world = world;
      this.pinFirst = pinFirst;
      this.pinLast = pinLast;
      this.drawIntermediatePoints = drawIntermediatePoints;
      points = new List<VerletPoint>();
    }

    public VerletChainBuilder AddPointAtPosition(float x, float y, PointConfigurator configurator = null)
    {
      return AddPointAtPosition(new Vector2(x, y), configurator);
    }

    public VerletChainBuilder AddPointAtPosition(Vector2 position, PointConfigurator configurator = null)
    {
      var point = world.CreatePoint();
      point.MoveToPosition(position);

      if (points.Count == 0)
      {
        point.PinToCurrentPosition();
      }
      else
      {
        if (!drawIntermediatePoints)
        {
          point.Drawing = false;
        }
      }

      if (configurator != null)
      {
        configurator(point);
      }

      points.Add(point);
      return this;
    }

    public VerletChainBuilder AddPointsAtOffset(int pointCount, float x, float y, PointConfigurator configurator = null)
    {
      return AddPointsAtOffset(pointCount, new Vector2(x, y), configurator);
    }

    public VerletChainBuilder AddPointsAtOffset(int pointCount, Vector2 offset, PointConfigurator configurator = null)
    {
      var builder = this;
      for (int i = 0; i < pointCount; ++i)
      {
        builder = builder.AddPointAtOffset(offset, configurator);
      }
      return builder;
    }

    public VerletChainBuilder AddPointAtOffset(float x, float y, PointConfigurator configurator = null)
    {
      return AddPointAtOffset(new Vector2(x, y), configurator);
    }

    public VerletChainBuilder AddPointAtOffset(Vector2 offset, PointConfigurator configurator = null)
    {
      Vector2 prevPosition = Vector2.Zero;
      if (points.Count > 0)
      {
        prevPosition = points[points.Count - 1].GlobalPosition;
      }

      return AddPointAtPosition(prevPosition + offset, configurator);
    }

    public void Build(float restingDistance = 50, float tearSensitivity = 100, float stiffness = 1)
    {
      if (points.Count < 2)
      {
        GD.PrintErr("Bad points length for chain. Need to be >= 2");
        return;
      }

      // Pin last
      var lastPoint = points[points.Count - 1];
      if (pinLast)
      {
        lastPoint.PinToCurrentPosition();
      }

      // Show last
      lastPoint.Drawing = true;

      int linkCount = points.Count - 1;
      VerletPoint prevPoint = points[0];
      for (int i = 1; i < points.Count; ++i)
      {
        var currPoint = points[i];

        var link = world.CreateLink(prevPoint, currPoint);
        link.RestingDistance = restingDistance;
        link.TearSensitivity = tearSensitivity;
        link.Stiffness = stiffness;

        prevPoint = currPoint;
      }
    }
  }
}
