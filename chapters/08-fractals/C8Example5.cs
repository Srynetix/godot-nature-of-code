using Godot;
using Fractals;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example5 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.5:\nKoch curve";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var start = new Vector2(20, (size.y / 1.25f) - 20);
                var end = new Vector2(size.x - 20, (size.y / 1.25f) - 20);
                var kochCurveNode = new KochCurveNode() {
                    KochCurve = new KochCurve(start, end, 6)
                };
                AddChild(kochCurveNode);
            }
        }
    }
}
