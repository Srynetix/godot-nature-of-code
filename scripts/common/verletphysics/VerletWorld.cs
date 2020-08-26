using Godot;
using System.Collections.Generic;

// Inspired from:
// https://gamedevelopment.tutsplus.com/tutorials/simulate-tearable-cloth-and-ragdolls-with-simple-verlet-integration--gamedev-519

namespace VerletPhysics
{
  public class VerletWorld : Node2D
  {
    public int ConstraintAccuracy = 2;
    public Vector2 Gravity = new Vector2(0, 9.81f);

    private List<VerletPoint> points;
    private List<VerletLink> linksToRemove;

    public VerletWorld()
    {
      points = new List<VerletPoint>();
      linksToRemove = new List<VerletLink>();
    }

    public VerletPoint CreatePoint()
    {
      var point = new VerletPoint(this);
      points.Add(point);
      AddChild(point);

      return point;
    }

    public VerletLink CreateLink(VerletPoint A, VerletPoint B)
    {
      var link = new VerletLink(this, A, B);
      A.AddLink(link);
      AddChild(link);
      return link;
    }

    public VerletChainBuilder StartChainBuilder(bool pinFirst = true, bool pinLast = false, bool drawIntermediatePoints = false)
    {
      return new VerletChainBuilder(this, pinFirst, pinLast, drawIntermediatePoints);
    }

    public void QueueLinkRemoval(VerletLink link)
    {
      if (!linksToRemove.Contains(link))
      {
        linksToRemove.Add(link);
      }
    }

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
          point.ApplyConstraints(size);
        }
      }

      foreach (VerletPoint point in points)
      {
        point.ApplyMovement(Gravity, delta);
      }
    }

    public override void _PhysicsProcess(float delta)
    {
      ProcessPoints(delta);

      foreach (VerletLink link in linksToRemove)
      {
        RemoveLink(link);
      }
      linksToRemove.Clear();
    }
  }
}
