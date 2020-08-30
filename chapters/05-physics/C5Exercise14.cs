using Godot;
using VerletPhysics;

public class C5Exercise14 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.14:\n"
      + "Verlet Creature\n\n"
      + "You can move points by touching them\n"
      + "If you drag points quick enough, links will break";
  }

  private class VerletCreature
  {
    public VerletCreature(VerletWorld world, Vector2 centerPosition, float gravityScale = 1)
    {
      var pointCount = 8;
      var diameter = 50;
      var stiffness = 0.1f;

      var cluster1 = new VerletCluster(
        world,
        centerPosition - new Vector2(50, 0),
        pointCount: pointCount,
        diameter: diameter,
        gravityScale: gravityScale,
        tearSensitivityFactor: 4,
        stiffness: stiffness
      );

      var cluster2 = new VerletCluster(
        world,
        centerPosition + new Vector2(50, 0),
        pointCount: pointCount,
        diameter: diameter,
        gravityScale: gravityScale,
        tearSensitivityFactor: 4,
        stiffness: stiffness
      );

      var cluster3 = new VerletCluster(
        world,
        centerPosition - new Vector2(0, 50),
        pointCount: pointCount,
        diameter: diameter,
        gravityScale: gravityScale,
        tearSensitivityFactor: 4,
        stiffness: stiffness
      );

      var linkStiffness = 0.75f;
      var linkDistance = diameter * 3;
      for (int i = 0; i < cluster1.Points.Count; ++i)
      {
        world.CreateLink(cluster1.Points[i], cluster2.Points[i], restingDistance: linkDistance, stiffness: linkStiffness, tearSensitivityFactor: 2);
        world.CreateLink(cluster2.Points[i], cluster3.Points[i], restingDistance: linkDistance, stiffness: linkStiffness, tearSensitivityFactor: 2);
        world.CreateLink(cluster3.Points[i], cluster1.Points[i], restingDistance: linkDistance, stiffness: linkStiffness, tearSensitivityFactor: 2);
      }
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var physics = new VerletWorld();
    AddChild(physics);

    var creature = new VerletCreature(physics, size / 2, gravityScale: 0.5f);

    var ragdoll1 = new VerletRagdoll(
      physics,
      new Vector2(size.x / 4, size.y / 2),
      height: 200,
      pointRadius: 5f,
      gravityScale: 0.25f,
      tearSensitivityFactor: 4,
      drawIntermediatePoints: false,
      drawSupportLinks: false
    );

    var ragdoll2 = new VerletRagdoll(
      physics,
      new Vector2(size.x * 0.75f, size.y / 2),
      height: 200,
      pointRadius: 5f,
      gravityScale: 0.25f,
      tearSensitivityFactor: 4,
      drawIntermediatePoints: false,
      drawSupportLinks: false
    );
  }
}
