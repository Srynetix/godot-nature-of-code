using Godot;
using Fractals;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise13 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.13:\nStochastic L-System\n\nClick on screen to display next generation";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var turtle = new LSystemTurtleNode()
                {
                    LSystem = new StochasticLSystem()
                    {
                        Rules = new List<LSystemRule>() {
                            new LSystemRule() {
                                Input = 'F',
                                Output = "FF+[+F-F-F]-[-F+F+F]"
                            },
                            new LSystemRule() {
                                Input = 'F',
                                Output = "FF+[+F]-[-F-F]"
                            },
                            new LSystemRule() {
                                Input = 'F',
                                Output = "FF+[+F]-[-F-F-F]"
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
