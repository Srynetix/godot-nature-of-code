using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.4: Infinite 1D CA.
    /// </summary>
    public class C7Exercise4 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.4:\nInfinite 1D CA";
        }

        public override void _Ready()
        {
            AddChild(new CellularAutomata1D()
            {
                ScrollLines = true
            });
        }
    }
}
