using System.Linq;
using Godot;
using Forces;

namespace Examples.Chapter2
{
  /// <summary>
  /// Example 2.5 - Fluid resistance.
  /// </summary>
  /// Uses SimpleMover and SimpleLiquid to simulate fluid resistance.
  /// Behind the scenes, SimpleLiquid is based on Area2D overlap detection.
  public class C2Example5 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 2.5:\n"
        + "Fluid Resistance";
    }

    private class Mover : SimpleMover
    {
      public Mover() : base(WrapModeEnum.Bounce) { }

      protected override void UpdateAcceleration()
      {
        var wind = new Vector2(0.1f, 0);
        var gravity = new Vector2(0, 0.098f * Mass);

        ApplyForce(wind);
        ApplyForce(gravity);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var zone = new SimpleLiquid
      {
        Coeff = 0.25f,
        Size = new Vector2(size.x, size.y / 4),
        Position = new Vector2(size.x / 2, size.y - (size.y / 8))
      };
      AddChild(zone);

      foreach (var _ in Enumerable.Range(0, 20))
      {
        var mover = new Mover();
        var bodySize = (float)GD.RandRange(20, 40);
        mover.MeshSize = new Vector2(bodySize, bodySize);
        mover.Mass = (float)GD.RandRange(5, 10);
        var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
        mover.Position = new Vector2(xPos, size.y / 2);
        AddChild(mover);
      }
    }
  }
}
