using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise7 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.7:\nRecursive tree with weight";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawTree(new Vector2(size.x / 2, size.y / 1.15f), 0, 125, 10);
            }

            private void DrawTree(Vector2 position, float rotation, float length, float weight)
            {
                if (length <= 2)
                {
                    return;
                }

                var start = position;
                var end = position + new Vector2(0, -length).Rotated(rotation);
                var newRotation = Mathf.Pi / 6;
                var newLength = length * 0.66f;
                var newWeight = weight * 0.66f;

                DrawLine(start, end, Colors.White, weight);
                DrawTree(end, rotation + newRotation, newLength, newWeight);
                DrawTree(end, rotation - newRotation, newLength, newWeight);
            }
        }
    }
}
