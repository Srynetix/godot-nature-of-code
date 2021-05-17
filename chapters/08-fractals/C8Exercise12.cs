using Godot;
using Fractals;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise12 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.12:\nL-System with other fractals\n\nClick on screen to display next generation";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var curveNode = new LSystemCurveNode()
                {
                    Position = new Vector2(60, size.y - 60)
                };

                AddChild(curveNode);
            }

            public class LSystemCurveNode : LSystemTurtleNode
            {
                public LSystemCurveNode()
                {
                    LSystem = new LSystem()
                    {
                        Rules = new List<LSystemRule>() {
                            new LSystemRule() {
                                Input = 'F',
                                Output = "F-F++F-F"
                            }
                        },
                        Current = "F"
                    };
                    Theta = Mathf.Pi / 4;
                    LengthMultiplicator = 0.6f;
                    InitialRotation = Mathf.Pi / 2;
                }
            }
        }
    }
}
