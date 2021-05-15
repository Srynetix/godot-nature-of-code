using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example2 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.2:\nRecursion twice";
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
                    RecursiveDrawCircle(position + new Vector2(radius, 0), radius / 2);
                    RecursiveDrawCircle(position - new Vector2(radius, 0), radius / 2);
                }
            }
        }
    }
}
