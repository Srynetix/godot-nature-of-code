using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.13: Historical Game of Life.
    /// </summary>
    public class C7Exercise13 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.13:\nHistorical Game of Life\n\nCells darken when alive for too long.";
        }

        public override void _Ready()
        {
            var ca = new HistoricalGameOfLife()
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
            };
            AddChild(ca);

            ca.RandomizeGrid();
        }
    }
}
