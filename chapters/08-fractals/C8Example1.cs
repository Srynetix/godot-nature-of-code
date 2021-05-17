using Godot;

namespace Examples
{
    /// <summary>
    /// Chapter 8 - Fractals.
    /// </summary>
    namespace Chapter8
    {
        /// <summary>
        /// Example 8.1: Recursive Circles 1.
        /// </summary>
        public class C8Example1 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.1:\nRecursive Circles 1";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                RecursiveDrawCircle(size / 2, size.x);
            }

            private void RecursiveDrawCircle(Vector2 position, float radius)
            {
                DrawArc(position, radius, 0, 2 * Mathf.Pi, 64, Colors.White);
                if (radius > 2)
                {
                    radius *= 0.75f;
                    RecursiveDrawCircle(position, radius);
                }
            }
        }
    }
}
