using System.Linq;
using Godot;
using Drawing;
using Forces;

namespace Examples
{
  /// <summary>
  /// Exercise 2.8 - Attractor Pattern.
  /// </summary>
  /// Uses SimpleAttractors, SimpleMovers and SimpleTrails to draw patterns.
  public class C2Exercise8 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 2.8:\n"
        + "Attractor Pattern";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var attractor1 = new SimpleAttractor();
      attractor1.Position = new Vector2(size.x / 4, size.y / 2);
      AddChild(attractor1);

      var attractor2 = new SimpleAttractor();
      attractor2.Position = new Vector2(size.x / 2, size.y / 2);
      AddChild(attractor2);

      var attractor3 = new SimpleAttractor();
      attractor3.Position = new Vector2(size.x - size.x / 4, size.y / 2);
      AddChild(attractor3);

      foreach (var x in Enumerable.Range(0, 10))
      {
        var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
        var bodySize = (float)GD.RandRange(20, 40);
        var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
        var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
        mover.MeshSize = new Vector2(bodySize, bodySize);
        mover.Mass = bodySize;
        mover.Position = new Vector2(xPos, yPos);

        var trail = new SimpleTrail();
        trail.Target = mover;

        AddChild(trail);
        AddChild(mover);
      }
    }
  }
}
