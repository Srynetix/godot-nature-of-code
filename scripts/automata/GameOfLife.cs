namespace Automata
{
    /// <summary>
    /// Boolean cell.
    /// </summary>
    public class BoolCell : Cell<bool>
    {
        public override bool GetAliveValue()
        {
            return true;
        }

        public override bool GetDeadValue()
        {
            return false;
        }
    }

    /// <summary>
    /// Boolean cellular automata 2D.
    /// </summary>
    public class GameOfLife : CellularAutomata2D<BoolCell, bool> { }
}
