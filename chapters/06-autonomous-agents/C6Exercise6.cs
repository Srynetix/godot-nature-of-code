using Godot;
using Agents;

namespace Examples.Chapter6
{
    /// <summary>
    /// Exercise 6.6: Radial Flow Field.
    /// </summary>
    /// Uses SimpleFlowField.
    public class C6Exercise6 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 6.6:\nRadial Flow Field";
        }

        private class RadialFlowField : SimpleFlowField
        {
            protected override Vector2 ComputeDirectionFromPosition(int x, int y)
            {
                var center = GetViewportRect().Size / 2;
                var pos = new Vector2(x, y) * Resolution;
                return (center - pos).Normalized();
            }
        }

        public override void _Ready()
        {
            var flowField = new RadialFlowField() { Resolution = 30 };
            AddChild(flowField);
        }
    }
}
