using Godot;

namespace Examples.Chapter1
{
    /// <summary>
    /// Example 1.3 - Vector subtraction.
    /// </summary>
    /// Uses _Draw to represent vector subtraction.
    public class C1Example3 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 1.3:\n"
              + "Vector subtraction";
        }

        public override void _Draw()
        {
            var mousePosition = GetViewport().GetMousePosition();
            var size = GetViewportRect().Size;
            var center = size / 2;

            var target = mousePosition - center;

            DrawLine(center, center + target, Colors.LightBlue, 2, true);
        }

        public override void _Process(float delta)
        {
            Update();
        }
    }
}
