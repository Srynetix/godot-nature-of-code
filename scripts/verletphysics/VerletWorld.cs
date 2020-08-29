using Godot;
using System.Collections.Generic;

// Inspired from:
// https://gamedevelopment.tutsplus.com/tutorials/simulate-tearable-cloth-and-ragdolls-with-simple-verlet-integration--gamedev-519

namespace VerletPhysics
{
  /// <summary>
  /// Simple verlet world which handle simulation book-keeping.
  /// </summary>
  public class VerletWorld : Node2D
  {
    /// <summary>Constraint resolution accuracy</summary>
    public int ConstraintAccuracy = 2;
    /// <summary>World gravity</summary>
    public Vector2 Gravity = new Vector2(0, 9.81f);

    private List<VerletPoint> points;
    private List<VerletLink> linksToRemove;

    /// <summary>
    /// Create a default verlet world.
    /// </summary>
    public VerletWorld()
    {
      points = new List<VerletPoint>();
      linksToRemove = new List<VerletLink>();
    }

    /// <summary>
    /// Create a verlet point.
    /// </summary>
    /// <returns>Verlet point</returns>
    public VerletPoint CreatePoint()
    {
      var point = new VerletPoint(this);
      points.Add(point);
      AddChild(point);

      return point;
    }

    /// <summary>
    /// Create a verlet link.
    /// </summary>
    /// <param name="a">First verlet point</param>
    /// <param name="b">First verlet point</param>
    /// <returns>Verlet link</returns>
    public VerletLink CreateLink(VerletPoint a, VerletPoint b)
    {
      var link = new VerletLink(this, a, b);
      a.AddLink(link);
      AddChild(link);
      return link;
    }

    /// <summary>
    /// Start a verlet chain builder.
    /// </summary>
    /// <param name="pinFirst">Pin first chain point</param>
    /// <param name="pinLast">Pin last chain point</param>
    /// <param name="drawIntermediatePoints">Draw chain intermediate points</param>
    /// <returns>Verlet chain builder</returns>
    public VerletChainBuilder StartChainBuilder(bool pinFirst = true, bool pinLast = false, bool drawIntermediatePoints = false)
    {
      return new VerletChainBuilder(this, pinFirst, pinLast, drawIntermediatePoints);
    }

    /// <summary>
    /// Start a verlet cloth builder.
    /// </summary>
    /// <param name="pinMode">Pin mode</param>
    /// <param name="drawPoints">Draw verlet points</param>
    /// <param name="pointRadius">Verlet point radius</param>
    /// <returns>Verlet cloth builder</returns>
    public VerletClothBuilder StartClothBuilder(VerletClothBuilder.PinModeEnum pinMode = VerletClothBuilder.PinModeEnum.TopCorners, bool drawPoints = false, float pointRadius = 10f)
    {
      return new VerletClothBuilder(this, pinMode, drawPoints, pointRadius);
    }

    /// <summary>
    /// Start a verlet cluster builder.
    /// </summary>
    /// <param name="drawPoints">Draw verlet points</param>
    /// <param name="pointRadius">Verlet point radius</param>
    /// <returns>Verlet cluster builder</returns>
    public VerletClusterBuilder StartClusterBuilder(bool drawPoints = false, float pointRadius = 10f)
    {
      return new VerletClusterBuilder(this, drawPoints, pointRadius);
    }

    /// <summary>
    /// Mark a link to be removed next frame.
    /// </summary>
    /// <param name="link">Verlet link</param>
    public void QueueLinkRemoval(VerletLink link)
    {
      if (!linksToRemove.Contains(link))
      {
        linksToRemove.Add(link);
      }
    }

    #region Lifecycle methods

    public override void _PhysicsProcess(float delta)
    {
      ProcessPoints(delta);

      foreach (VerletLink link in linksToRemove)
      {
        RemoveLink(link);
      }
      linksToRemove.Clear();
    }

    #endregion

    #region Private methods

    private void RemoveLink(VerletLink link)
    {
      foreach (VerletPoint p in points)
      {
        p.RemoveLink(link);
      }

      RemoveChild(link);
    }

    private void ProcessPoints(float delta)
    {
      var size = GetViewportRect().Size;

      for (int i = 0; i < ConstraintAccuracy; ++i)
      {
        foreach (VerletPoint point in points)
        {
          point.Constraint(size);
        }
      }

      foreach (VerletPoint point in points)
      {
        point.UpdateMovement(Gravity, delta);
      }
    }

    #endregion
  }
}
