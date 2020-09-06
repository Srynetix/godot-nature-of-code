using Godot;
using System.Collections.Generic;
using Oscillation;

namespace Examples.Chapter3
{
  /// <summary>
  /// Exercise 3.12 - Pendulum Chain.
  /// </summary>
  /// Uses a SimplePendulum tree.
  public class C3Exercise12 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 3.12:\n"
        + "Pendulum Chain\n\n"
        + "You can move pendulums by touching them.";
    }

    public float Steps = 3;
    public float BaseAngle = Mathf.Pi / 2;
    public float ChildrenPerParent = 2;

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var angleStep = BaseAngle / ChildrenPerParent;
      var ropeLength = (size.y * 0.95f) / Steps;
      List<SimplePendulum> parents = new List<SimplePendulum>();

      for (int step = 0; step < Steps; ++step)
      {
        List<SimplePendulum> newParents = new List<SimplePendulum>();

        if (parents.Count == 0)
        {
          // Root pendulum
          var plm = new SimplePendulum
          {
            Position = new Vector2(size.x / 2, 0),
            RopeLength = ropeLength,
            Angle = Mathf.Pi / 2
          };
          AddChild(plm);
          newParents.Add(plm);
        }
        else
        {
          // Child pendulums
          for (int pIndex = 0; pIndex < parents.Count; ++pIndex)
          {
            for (int cIndex = 0; cIndex < ChildrenPerParent; ++cIndex)
            {
              var plm = new SimplePendulum
              {
                ShowBehindParent = true,
                RopeLength = ropeLength,
                Angle = angleStep * cIndex
              };
              parents[pIndex].AddPendulumChild(plm);
              newParents.Add(plm);
            }
          }
        }

        parents = newParents;
      }
    }
  }
}
