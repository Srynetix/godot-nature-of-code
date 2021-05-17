using Godot;
using Fractals;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example10 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.10:\nL-System tree with mouse\n\nClick on screen to display next generation";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var turtle = new LSystemTurtleNode()
                {
                    LSystem = new LSystem()
                    {
                        Rules = new List<LSystemRule>() {
                            new LSystemRule() {
                                Input = 'F',
                                Output = "FF+[+F-F-F]-[-F+F+F]"
                            }
                        },
                        Current = "F"
                    }
                };
                turtle.Position = new Vector2(size.x / 2, size.y - 20);
                AddChild(turtle);
            }
        }
    }
}
