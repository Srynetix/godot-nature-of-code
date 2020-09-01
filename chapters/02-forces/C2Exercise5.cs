using System.Linq;
using Godot;
using Forces;

namespace Examples
{
  /// <summary>
  /// Exercise 2.5 - Water Drag Height.
  /// </summary>
  /// Shows water drag depending on SimpleMovers height.
  public class C2Exercise5 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 2.5:\n"
        + "Water Drag Height";
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

      var zone = new SimpleLiquid();
      zone.Coeff = 0.25f;
      zone.Size = new Vector2(size.x, size.y / 4);
      zone.Position = new Vector2(size.x / 2, size.y - size.y / 8);
      AddChild(zone);

      foreach (var x in Enumerable.Range(0, 20))
      {
        var mover = new Mover();
        var bodySize = (float)GD.RandRange(20, 40);
        var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
        mover.MeshSize = new Vector2(bodySize, bodySize);
        mover.Mass = (float)GD.RandRange(5, 10);
        mover.Position = new Vector2(xPos, size.y / 2 + (float)GD.RandRange(-100, 100));
        AddChild(mover);
      }
    }
  }
}
