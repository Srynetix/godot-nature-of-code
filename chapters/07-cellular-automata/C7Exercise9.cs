using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.9: Hexagonal Game of Life.
    /// </summary>
    public class C7Exercise9 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.9:\nHexagonal Game of Life";
        }

        public override void _Ready()
        {
            var ca = new HexagonalGameOfLife()
            {
                WrapBehavior = WrapBehaviorEnum.Wrap,
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                HighlightTransitions = true
            };
            AddChild(ca);

            ca.RandomizeGrid();
        }
    }
}
