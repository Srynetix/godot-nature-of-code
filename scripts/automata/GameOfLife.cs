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
    public class GameOfLife : CellularAutomata2D<BoolCell, bool>
    {
        /// <summary>
        /// Create a standard game of life of scale 20.
        /// </summary>
        public GameOfLife() : this(20) { }

        /// <summary>
        /// Create a Game of Life instance of custom scale.
        /// </summary>
        /// <param name="scale">Scale</param>
        public GameOfLife(int scale) : base(scale) { }
    }
}
