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
    public VerletWorld(Vector2? gravity = null)
    {
      points = new List<VerletPoint>();
      linksToRemove = new List<VerletLink>();
      Gravity = gravity ?? Gravity;
    }

    /// <summary>
    /// Create a verlet point.
    /// </summary>
    /// <param name="initialPosition">Initial position</param>
    /// <param name="mass">Mass</param>
    /// <param name="radius">Radius</param>
    /// <param name="visible">Show point</param>
    /// <returns>Verlet point</returns>
    public VerletPoint CreatePoint(Vector2? initialPosition = null, float? mass = null, float? radius = null, bool? visible = null)
    {
      var point = new VerletPoint(this);
      points.Add(point);
      AddChild(point);

      point.Mass = mass ?? point.Mass;
      point.Radius = radius ?? point.Radius;
      point.Visible = visible ?? point.Visible;

      if (initialPosition.HasValue)
      {
        point.MoveToPosition(initialPosition.Value);
      }

      return point;
    }

    /// <summary>
    /// Create a verlet link.
    /// </summary>
    /// <param name="a">First verlet point</param>
    /// <param name="b">First verlet point</param>
    /// <param name="restingDistance">Resting distance</param>
    /// <param name="tearSensitivity">Distance required to break the link. Use `-1` to create an unbreakable link.</param>
    /// <param name="tearSensitivityFactor">Distance factor required to break the link. Use `-1` to create an unbreakable link.</param>
    /// <param name="stiffness">Stiffness of the link</param>
    /// <param name="color">Link color</param>
    /// <param name="visible">Show link</param>
    /// <returns>Verlet link</returns>
    public VerletLink CreateLink(VerletPoint a, VerletPoint b, float? restingDistance = null, float? tearSensitivity = null, float? tearSensitivityFactor = null, float? stiffness = null, Color? color = null, bool? visible = null)
    {
      var link = new VerletLink(this, a, b);
      a.AddLink(link);
      AddChild(link);

      link.Visible = visible ?? link.Visible;
      link.RestingDistance = restingDistance ?? link.RestingDistance;
      link.TearSensitivity = tearSensitivity ?? link.TearSensitivity;
      link.Stiffness = stiffness ?? link.Stiffness;
      link.Modulate = color ?? link.Modulate;

      if (tearSensitivityFactor.HasValue)
      {
        link.TearSensitivity = link.RestingDistance * tearSensitivityFactor.Value;
      }

      return link;
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
