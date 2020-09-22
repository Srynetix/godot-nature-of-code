using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.6: Game of Life cell draw.
    /// </summary>
    public class C7Exercise6 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.6:\nGame of Life Cell Draw";
        }

        public override void _Ready()
        {
            var ca = new BoolCellularAutomata2D();
            AddChild(ca);

            ca.TouchBehavior = TouchBehaviorEnum.DrawCell;
            ca.WrapBehavior = WrapBehaviorEnum.Nowrap;
            ca.RandomizeGrid();
        }
    }
}
