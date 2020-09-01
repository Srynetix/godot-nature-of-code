using Godot;
using System.Collections.Generic;

/// <summary>
/// Verlet physics implementation.
/// Inspired from `toxiclibs` and https://gamedevelopment.tutsplus.com/tutorials/simulate-tearable-cloth-and-ragdolls-with-simple-verlet-integration--gamedev-519
/// </summary>
namespace VerletPhysics
{
  /// <summary>
  /// Simple verlet chain builder.
  /// Uses a `PointConfiguratorFunc` to configure `VerletPoints` on creation.
  /// </summary>
  public class VerletChainBuilder
  {
    /// <summary>Point configuration function definition</summary>
    public delegate void PointConfiguratorFunc(VerletPoint point);

    private bool pinFirst;
    private bool pinLast;
    private bool drawIntermediatePoints;
    private VerletWorld world;
    private List<VerletPoint> points;

    /// <summary>
    /// Create a verlet chain builder.
    /// </summary>
    /// <param name="world">Verlet world</param>
    /// <param name="pinFirst">Pin first chain point</param>
    /// <param name="pinLast">Pin last chain point</param>
    /// <param name="drawIntermediatePoints">Draw chain intermediate points</param>
    public VerletChainBuilder(VerletWorld world, bool pinFirst = true, bool pinLast = false, bool drawIntermediatePoints = false)
    {
      this.world = world;
      this.pinFirst = pinFirst;
      this.pinLast = pinLast;
      this.drawIntermediatePoints = drawIntermediatePoints;
      points = new List<VerletPoint>();
    }

    /// <summary>
    /// Add chain point at a specific position.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointAtPosition(float x, float y, PointConfiguratorFunc configurator = null)
    {
      return AddPointAtPosition(new Vector2(x, y), configurator);
    }

    /// <summary>
    /// Add chain point at a specific position.
    /// </summary>
    /// <param name="position">Position vector</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointAtPosition(Vector2 position, PointConfiguratorFunc configurator = null)
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
          point.Visible = false;
        }
      }

      if (configurator != null)
      {
        configurator(point);
      }

      points.Add(point);
      return this;
    }

    /// <summary>
    /// Add chain point with offset from previous point position.
    /// If not already set, the first point will be offset from (0, 0).
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointWithOffset(float x, float y, PointConfiguratorFunc configurator = null)
    {
      return AddPointWithOffset(new Vector2(x, y), configurator);
    }

    /// <summary>
    /// Add chain point with offset from previous point position.
    /// If not already set, the first point will be offset from (0, 0).
    /// </summary>
    /// <param name="offset">Offset vector</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointWithOffset(Vector2 offset, PointConfiguratorFunc configurator = null)
    {
      Vector2 prevPosition = Vector2.Zero;
      if (points.Count > 0)
      {
        prevPosition = points[points.Count - 1].GlobalPosition;
      }

      return AddPointAtPosition(prevPosition + offset, configurator);
    }

    /// <summary>
    /// Add multiple chain points with offset from previous points positions.
    /// If not already set, the first point will be offset from (0, 0).
    /// </summary>
    /// <param name="pointCount">Point count</param>
    /// <param name="x">X offset</param>
    /// <param name="y">Y offset</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointsWithOffset(int pointCount, float x, float y, PointConfiguratorFunc configurator = null)
    {
      return AddPointsWithOffset(pointCount, new Vector2(x, y), configurator);
    }

    /// <summary>
    /// Add multiple chain points with offset from previous points positions.
    /// If not already set, the first point will be offset from (0, 0).
    /// </summary>
    /// <param name="pointCount">Point count</param>
    /// <param name="offset">Offset vector</param>
    /// <param name="configurator">Point configurator</param>
    /// <returns>Builder</returns>
    public VerletChainBuilder AddPointsWithOffset(int pointCount, Vector2 offset, PointConfiguratorFunc configurator = null)
    {
      var builder = this;
      for (int i = 0; i < pointCount; ++i)
      {
        builder = builder.AddPointWithOffset(offset, configurator);
      }
      return builder;
    }

    /// <summary>
    /// Build the verlet chain.
    /// </summary>
    /// <param name="restingDistance">Resting distance</param>
    /// <param name="tearSensitivity">Distance required to break the chain. Use `-1` to create an unbreakable chain.</param>
    /// <param name="stiffness">Stiffness of the chain</param>
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
      lastPoint.Visible = true;

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
