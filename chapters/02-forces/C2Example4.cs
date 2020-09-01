using System.Linq;
using Godot;
using Forces;

namespace Examples
{
  namespace Chapter2
  {
    /// <summary>
    /// Example 2.4 - Friction.
    /// </summary>
    /// Uses SimpleMover ApplyFriction method.
    public class C2Example4 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 2.4:\n"
          + "Friction";
      }

      private class Mover : SimpleMover
      {
        public Mover() : base(WrapModeEnum.Bounce) { }

        protected override void UpdateAcceleration()
        {
          var wind = new Vector2(0.01f, 0);
          var gravity = new Vector2(0, 0.09f * Mass);

          ApplyForce(wind);
          ApplyForce(gravity);
          ApplyFriction(0.1f);
        }
      }

      public override void _Ready()
      {
        foreach (var x in Enumerable.Range(0, 20))
        {
          var mover = new Mover();
          var bodySize = (float)GD.RandRange(20, 40);
          mover.MeshSize = new Vector2(bodySize, bodySize);
          mover.Mass = (float)GD.RandRange(5, 10);

          var size = GetViewportRect().Size;
          var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
          mover.Position = new Vector2(xPos, size.y / 2);

          AddChild(mover);
        }
      }
    }
  }
}
