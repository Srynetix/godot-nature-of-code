using Godot;
using Agents;

namespace Examples.Chapter6
{
    /// <summary>
    /// Example 6.5: Simple Path Following.
    /// </summary>
    /// Uses SimplePath.
    public class C6Example5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 6.5:\nSimple Path Following";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var path = new SimplePath();
            path.Points.Add(new Vector2(0, size.y / 3));
            path.Points.Add(new Vector2(size.x, 2 * size.y / 3));
            AddChild(path);

            var vehicle = new SimpleVehicle
            {
                TargetPath = path,
                Velocity = new Vector2(10, 0),
                Position = new Vector2(100, 100)
            };
            AddChild(vehicle);
        }
    }
}
