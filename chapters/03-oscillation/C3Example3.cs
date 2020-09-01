using Godot;
using Drawing;
using Forces;

namespace Examples
{
  namespace Chapter3
  {
    /// <summary>
    /// Example 3.3 - Pointing towards motion.
    /// </summary>
    /// Update SimpleMover rotation depending on mouse position.
    public class C3Example3 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 3.3:\n"
          + "Pointing towards motion";
      }

      private class Mover : SimpleMover
      {
        public Mover()
        {
          Mesh.MeshType = SimpleMesh.TypeEnum.Custom;
          Mesh.CustomDrawMethod = (pen) =>
          {
            var length = Radius * 2;
            var width = Radius;

            pen.DrawRect(new Rect2(-length / 2, -width / 2, length, width), pen.Modulate);
          };
        }

        protected override void UpdateAcceleration()
        {
          var mousePos = GetViewport().GetMousePosition();
          var dir = (mousePos - Position).Normalized();

          Acceleration = dir * 0.5f;
        }

        public override void _Process(float delta)
        {
          base._Process(delta);

          // Update angle from velocity
          float angle = Mathf.Atan2(Velocity.y, Velocity.x);
          Rotation = angle;
        }
      }

      public override void _Ready()
      {
        GD.Randomize();
        var size = GetViewportRect().Size;

        var mover = new Mover();
        mover.Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y));
        AddChild(mover);
      }
    }
  }
}
