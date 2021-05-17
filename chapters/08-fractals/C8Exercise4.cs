using Godot;
using Fractals;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise4 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.4:\nCantor set class";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var start = new Vector2(25, size.y / 3);
                var end = new Vector2(size.x - 25, start.y);
                var cantorSetNode = new CantorSetNode()
                {
                    CantorSet = new CantorSet(start, end, 6, 20)
                };
                AddChild(cantorSetNode);
            }
        }
    }
}
