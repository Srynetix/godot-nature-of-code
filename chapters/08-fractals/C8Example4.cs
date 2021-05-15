using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example4 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.4:\nCantor set";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                var length = size.x - 50;
                Cantor(new Vector2(25, size.y / 3), length, separation: 20);
            }

            private void Cantor(Vector2 position, float length, float separation) {
                if (length >= 1) {
                    DrawLine(position, position + new Vector2(length, 0), Colors.White, width: 10);

                    var newPosition = position + new Vector2(0, separation);
                    Cantor(newPosition, length / 3, separation);
                    Cantor(newPosition + new Vector2(length * 2/3.0f, 0), length / 3, separation);
                }
            }
        }
    }
}
