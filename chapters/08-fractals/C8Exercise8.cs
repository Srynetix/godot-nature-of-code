using Godot;
using Fractals;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise8 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.8:\nRecursive tree object";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var position = new Vector2(size.x / 2, size.y / 1.15f);
                const int length = 125;
                const int weight = 10;

                var tree = new RecursiveTreeNode()
                {
                    RecursiveTree = new RecursiveTree(
                        new RecursiveBranch(position, position + new Vector2(0, -length), weight),
                        Mathf.Pi / 6,
                        5
                    )
                };
                AddChild(tree);
            }
        }
    }
}
