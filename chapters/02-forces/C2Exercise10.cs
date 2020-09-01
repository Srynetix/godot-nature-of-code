using System.Linq;

using Godot;
using Forces;

namespace Examples
{
  namespace Chapter2
  {
    /// <summary>
    /// Exercise 2.10 - Repulsion.
    /// </summary>
    /// Uses a custom SimpleAttractor with a reverse Attract method.
    /// Uses another custom SimpleAttractor which attract objects towards mouse.
    public class C2Exercise10 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 2.10:\n"
          + "Repulsion";
      }

      private class Repeller : SimpleAttractor
      {
        public override Vector2 Attract(SimpleMover mover)
        {
          return -base.Attract(mover);
        }
      }

      private class MouseAttractor : SimpleAttractor
      {
        private bool active = true;

        public override Vector2 Attract(SimpleMover mover)
        {
          if (!active)
          {
            return Vector2.Zero;
          }

          var mousePos = GetGlobalMousePosition();
          var mouseGravitation = 1;
          var mouseMass = 15;

          var force = mousePos - mover.GlobalPosition;
          var length = Mathf.Clamp(force.Length(), 5, 25);
          float strength = (mouseGravitation * mouseMass * mover.Mass) / (length * length);
          return force.Normalized() * strength;
        }

        public override void _Notification(int what)
        {
          if (what == NotificationWmMouseEnter)
          {
            active = true;
          }
          else if (what == NotificationWmMouseExit)
          {
            active = false;
          }
        }
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

          var repeller = new Repeller();
          repeller.Visible = false;
          mover.AddChild(repeller);

          var mouseAttractor = new MouseAttractor();
          mouseAttractor.Visible = false;
          AddChild(mouseAttractor);

          AddChild(mover);
        }
      }
    }
  }
}
