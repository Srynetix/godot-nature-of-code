using Godot;
using Agents;

namespace Examples.Chapter6
{
    /// <summary>
    /// Exercise 6.11: Animated Path.
    /// </summary>
    /// Uses a sine to move path.
    public class C6Exercise11 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 6.11:\nAnimated Path\n\nVehicle at the right will turn right and follow the path from left to right";
        }

        private SimplePath path;
        private float t;

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            path = new SimplePath();
            path.Points.Add(new Vector2(0, size.y / 2));
            path.Points.Add(new Vector2(size.x / 8, size.y * 3 / 4));
            path.Points.Add(new Vector2(size.x / 2, (size.y / 2) - 50));
            path.Points.Add(new Vector2(size.x, size.y / 2));
            AddChild(path);

            var vehicle1 = new SimpleVehicle
            {
                Position = new Vector2(100, 100),
                TargetPath = path
            };
            AddChild(vehicle1);

            var vehicle2 = new SimpleVehicle
            {
                Position = new Vector2(100, size.y - 100),
                TargetPath = path
            };
            AddChild(vehicle2);

            var vehicle3 = new SimpleVehicle
            {
                Velocity = new Vector2(-vehicle1.MaxVelocity, 0),
                Position = new Vector2(size.x - 100, size.y - 100),
                TargetPath = path
            };
            AddChild(vehicle3);
        }

        public override void _Process(float delta)
        {
            t += delta;

            // Sine
            for (int i = 0; i < path.Points.Count; ++i)
            {
                var point = path.Points[i];
                point.y += Mathf.Sin(point.x + t);
                path.Points[i] = point;
            }

            while (t > Mathf.Pi * 2)
            {
                t -= Mathf.Pi * 2;
            }
        }
    }
}
