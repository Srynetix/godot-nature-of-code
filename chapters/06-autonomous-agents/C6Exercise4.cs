using Godot;
using Agents;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Exercise 6.4: Reynolds' Wandering Behavior.
    /// </summary>
    /// Use a child node containing angle information.
    public class C6Exercise4 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 6.4:\nReynolds' Wandering Behavior";
      }

      private class ReynoldsVehicle : SimpleVehicle
      {
        private RandomCircle randomCircle;

        private class RandomCircle : Node2D
        {
          public float Distance = 60;
          public float Radius = 25;
          public float Theta = 0;

          public override void _Draw()
          {
            var circlePos = Vector2.Right * Distance;
            // Draw circle and line
            DrawLine(Vector2.Zero, circlePos, Colors.Black, 1);
            DrawCircle(circlePos, Radius, Colors.White.WithAlpha(32));
            DrawLine(circlePos, circlePos + Radius * new Vector2(Mathf.Cos(Theta), Mathf.Sin(Theta)), Colors.Black);
          }

          public override void _Process(float delta)
          {
            Theta += MathUtils.RandRangef(-10, 10) * delta;
            Update();
          }

          public Vector2 GetTarget()
          {
            return Distance * new Vector2(Mathf.Cos(Theta), Mathf.Sin(Theta));
          }
        }

        public ReynoldsVehicle()
        {
          randomCircle = new RandomCircle();
        }

        public override void _Ready()
        {
          base._Ready();

          // Reset offset
          randomCircle.Position = new Vector2(0, -Mesh.MeshSize.y / 4);
          AddChild(randomCircle);
        }

        protected override void UpdateAcceleration()
        {
          Seek(GlobalPosition + randomCircle.GetTarget());
        }
      }

      #region Lifecycle methods

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var vehicle = new ReynoldsVehicle();
        vehicle.Position = size / 2;
        AddChild(vehicle);
      }

      #endregion
    }
  }
}
