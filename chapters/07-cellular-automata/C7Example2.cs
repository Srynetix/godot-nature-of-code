using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Example 7.2: Game of Life.
    /// </summary>
    public class C7Example2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 7.2:\nGame of Life";
        }

        public override void _Ready()
        {
            var ca = new GameOfLife();
            AddChild(ca);

            ca.TouchBehavior = TouchBehaviorEnum.RandomizeGrid;
            ca.WrapBehavior = WrapBehaviorEnum.Nowrap;
            ca.RandomizeGrid();
        }
    }
}
