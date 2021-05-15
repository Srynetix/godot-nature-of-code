using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise1 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.1:\nCustom pattern";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                var length = size.x;
                Pattern(new Vector2(0, size.y / 2), length);
            }

            private void Pattern(Vector2 position, float length)
            {
                if (length >= 20)
                {
                    DrawLine(position, position - new Vector2(length / 2, -length / 2), Colors.LightGray);
                    DrawLine(position, position - new Vector2(length / 2, length / 2), Colors.LightGray);

                    DrawArc(position, length / 2, 0, Mathf.Pi * 2, 32, Colors.LightCyan);
                    Pattern(position + new Vector2(20, 0), length / 1.075f);
                }
            }
        }
    }
}
