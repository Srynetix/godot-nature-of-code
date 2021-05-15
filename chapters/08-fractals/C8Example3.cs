using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example3 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.3:\nRecursion four times";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                RecursiveDrawCircle(size / 2, size.x / 4);
            }

            private void RecursiveDrawCircle(Vector2 position, float radius) {
                DrawArc(position, radius, 0, 2 * Mathf.Pi, 16, Colors.White);
                if (radius > 8) {
                    RecursiveDrawCircle(position + new Vector2(radius, 0), radius / 2);
                    RecursiveDrawCircle(position - new Vector2(radius, 0), radius / 2);
                    RecursiveDrawCircle(position + new Vector2(0, radius), radius / 2);
                    RecursiveDrawCircle(position - new Vector2(0, radius), radius / 2);
                }
            }
        }
    }
}
