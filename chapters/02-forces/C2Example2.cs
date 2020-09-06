using System.Linq;
using Godot;
using Forces;

namespace Examples.Chapter2
{
  /// <summary>
  /// Example 2.2 - Forces on many objects.
  /// </summary>
  /// Same principle as Example 2.1, but on many objects.
  public class C2Example2 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 2.2:\n"
        + "Forces on many objects";
    }

    private class Mover : SimpleMover
    {
      public Mover() : base(WrapModeEnum.Bounce) { }

      protected override void UpdateAcceleration()
      {
        var wind = new Vector2(0.05f, 0);
        var gravity = new Vector2(0, 0.2f);

        ApplyForce(wind);
        ApplyForce(gravity);
      }
    }

    public override void _Ready()
    {
      foreach (var _ in Enumerable.Range(0, 20))
      {
        var mover = new Mover();
        var bodySize = (float)GD.RandRange(20, 40);
        mover.MeshSize = new Vector2(bodySize, bodySize);
        mover.Mass = (float)GD.RandRange(5, 10);
        AddChild(mover);
      }
    }
  }
}
