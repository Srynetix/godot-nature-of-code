using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example8 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.8:\nStochastic tree";
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
                var newLength = length * 0.66f;

                DrawLine(start, end, Colors.White);

                var branchCount = MathUtils.RandRangei(1, 4);
                for (int i = 0; i < branchCount; ++i)
                {
                    var newRotation = MathUtils.RandRangef(-Mathf.Pi / 2, Mathf.Pi / 2);
                    DrawTree(end, rotation + newRotation, newLength);
                }
            }
        }
    }
}
