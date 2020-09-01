using System.Linq;
using Godot;
using Forces;

namespace Examples
{
  namespace Chapter2
  {
    /// <summary>
    /// Example 2.8 - Mutual Attraction. 
    /// </summary>
    /// By adding a SimpleAttractor as a child of SimpleMover, it will not try to attract its parent.
    public class C2Example8 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 2.8:\n"
          + "Mutual Attraction";
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        foreach (var x in Enumerable.Range(0, 20))
        {
          var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
          var bodySize = (float)GD.RandRange(20, 40);
          var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
          var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
          mover.MeshSize = new Vector2(bodySize, bodySize);
          mover.Mass = bodySize;
          mover.Position = new Vector2(xPos, yPos);

          // Add attractor on mover
          var attractor = new SimpleAttractor();
          attractor.Visible = false;
          mover.AddChild(attractor);

          AddChild(mover);
        }
      }
    }
  }
}
