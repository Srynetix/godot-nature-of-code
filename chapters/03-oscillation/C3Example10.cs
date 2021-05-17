using Godot;
using Oscillation;

namespace Examples.Chapter3
{
    /// <summary>
    /// Example 3.10 - Swinging Pendulum.
    /// </summary>
    /// Uses SimplePendulum.
    public class C3Example10 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 3.10:\n"
              + "Swinging Pendulum\n\n"
              + "You can move the pendulum by touching it.";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;
            var pendulum = new SimplePendulum()
            {
                Angle = Mathf.Pi / 4,
                RopeLength = size.y / 2,
                Position = new Vector2(size.x / 2, 0)
            };
            AddChild(pendulum);
        }
    }
}
