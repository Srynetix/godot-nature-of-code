using Godot;

namespace Examples.Chapter1
{
    /// <summary>
    /// Example 1.4 - Vector multiplication.
    /// </summary>
    /// Uses _Draw to represent vector multiplication.
    public class C1Example4 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 1.4:\n"
              + "Vector multiplication";
        }

        public override void _Draw()
        {
            var mousePosition = GetViewport().GetMousePosition();
            var size = GetViewportRect().Size;
            var center = size / 2;

            var target = (mousePosition - center) * 0.5f;

            DrawLine(center, center + target, Colors.LightBlue, 2, true);
        }

        public override void _Process(float delta)
        {
            Update();
        }
    }
}
