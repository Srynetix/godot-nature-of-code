using Godot;
using Fractals;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise3 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.3:\nAnimated Koch curve";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var start = new Vector2(20, (size.y / 1.25f) - 20);
                var end = new Vector2(size.x - 20, (size.y / 1.25f) - 20);
                var kochCurveNode = new KochCurveNode() {
                    KochCurve = new KochCurve(start, end, 6),
                    Animated = true
                };
                AddChild(kochCurveNode);
            }
        }
    }
}
