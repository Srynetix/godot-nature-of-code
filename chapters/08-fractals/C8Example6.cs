using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example6 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.6:\nRecursive tree";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawTree(new Vector2(size.x / 2, size.y / 1.15f), 0, 125);
            }

            private void DrawTree(Vector2 position, float rotation, float length)
            {
                if (length <= 2)
                {
                    return;
                }

                var start = position;
                var end = position + new Vector2(0, -length).Rotated(rotation);
                var newRotation = Mathf.Pi / 6;
                var newLength = length * 0.66f;

                DrawLine(start, end, Colors.White);
                DrawTree(end, rotation + newRotation, newLength);
                DrawTree(end, rotation - newRotation, newLength);
            }
        }
    }
}
