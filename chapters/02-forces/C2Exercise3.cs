using System.Linq;
using Godot;
using Forces;

namespace Examples
{
  namespace Chapter2
  {
    /// <summary>
    /// Exercise 2.3 - Wind walls.
    /// </summary>
    /// Detect screen limits and apply a "wind" force high enough to push the movers toward the screen center.
    public class C2Exercise3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 2.3:\n"
          + "Wind walls";
      }

      private class Mover : SimpleMover
      {
        public Mover() : base(WrapModeEnum.None) { }

        public Vector2 ComputeWindForce()
        {
          var size = GetViewportRect().Size;
          var pos = Position;
          var output = Vector2.Zero;
          var limit = Radius * 8;

          // Push left
          if (Position.x > size.x - limit)
          {
            var force = limit * 2 - (size.x - Position.x);
            output.x = -force * 0.1f;
          }

          // Push right
          else if (Position.x < limit)
          {
            var force = limit * 2 - Position.x;
            output.x = force * 0.1f;
          }

          else
          {
            output.x = 0.1f;
          }

          // Push up
          if (Position.y > size.y - limit)
          {
            var force = limit * 2 - (size.y - Position.y);
            output.y = -force * 0.1f;
          }

          // Push down
          else if (Position.y < limit)
          {
            var force = limit * 2 - Position.y;
            output.y = force * 0.1f;
          }

          else
          {
            output.y = 0.9f;
          }

          return output;
        }

        protected override void UpdateAcceleration()
        {
          ApplyForce(ComputeWindForce());
        }
      }

      public override void _Ready()
      {
        foreach (var x in Enumerable.Range(0, 20))
        {
          var mover = new Mover();
          var bodySize = (float)GD.RandRange(20, 40);
          var size = GetViewportRect().Size;
          var xPos = (float)GD.RandRange(bodySize * 4, size.x - bodySize * 4);
          mover.MeshSize = new Vector2(bodySize, bodySize);
          mover.Mass = (float)GD.RandRange(5, 10);
          mover.Position = new Vector2(xPos, size.y / 2);
          AddChild(mover);
        }
      }
    }
  }
}
