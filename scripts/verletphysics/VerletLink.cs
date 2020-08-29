using Drawing;

namespace VerletPhysics
{
  /// <summary>
  /// Verlet breakable link.
  /// </summary>
  public class VerletLink : SimpleLineSprite
  {
    /// <summary>Resting distance</summary>
    public float RestingDistance = 100;
    /// <summary>Stiffness of the link</summary>
    public float Stiffness = 1;
    /// <summary>Distance required to break the link. Use `-1` to create an unbreakable link.</summary>
    public float TearSensitivity = 200;
    /// <summary>First verlet point</summary>
    public VerletPoint A;
    /// <summary>Second verlet point</summary>
    public VerletPoint B;

    private VerletWorld world;

    /// <summary>
    /// Create a default verlet link.
    /// </summary>
    /// <param name="world">Verlet world</param>
    /// <param name="a">First verlet point</param>
    /// <param name="b">Second verlet point</param>
    public VerletLink(VerletWorld world, VerletPoint a, VerletPoint b)
    {
      A = a;
      B = b;
      this.world = world;

      PositionA = A.GlobalPosition;
      PositionB = B.GlobalPosition;
    }

    /// <summary>
    /// Apply link constraint on the two verlet points.
    /// </summary>
    public void Constraint()
    {
      var diff = A.GlobalPosition - B.GlobalPosition;
      var d = diff.Length();
      var difference = (RestingDistance - d) / d;

      if (TearSensitivity != -1 && d > TearSensitivity)
      {
        world.QueueLinkRemoval(this);
      }

      var imA = 1 / A.Mass;
      var imB = 1 / B.Mass;
      var scalarA = (imA / (imA + imB)) * Stiffness;
      var scalarB = Stiffness - scalarA;

      A.GlobalPosition += diff * scalarA * difference;
      B.GlobalPosition -= diff * scalarB * difference;

      PositionA = A.GlobalPosition;
      PositionB = B.GlobalPosition;
    }

    #region Lifecycle methods

    public override void _Process(float delta)
    {
      PositionA = A.GlobalPosition;
      PositionB = B.GlobalPosition;
    }

    #endregion
  }
}
