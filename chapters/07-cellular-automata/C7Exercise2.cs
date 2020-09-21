using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.2: Random First Line.
    /// </summary>
    public class C7Exercise2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.2:\nRandom First Line";
        }

        private CellularAutomata1D ca;

        public override void _Ready()
        {
            ca = new CellularAutomata1D();
            AddChild(ca);

            ca.RandomizeCurrentLine();
            ca.Connect(nameof(CellularAutomata1D.ScreenCompleted), this, nameof(SetRandomRule));
        }

        private void SetRandomRule()
        {
            ca.RandomizeRuleSet();
            ca.RandomizeCurrentLine();
        }
    }
}
