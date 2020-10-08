using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.1: Random RuleSet.
    /// </summary>
    public class C7Exercise1 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.1:\nRandom RuleSet";
        }

        private CellularAutomata1D ca;

        public override void _Ready()
        {
            ca = new CellularAutomata1D();
            AddChild(ca);

            ca.Connect(nameof(CellularAutomata1D.ScreenCompleted), this, nameof(SetRandomRule));
        }

        private void SetRandomRule()
        {
            ca.RandomizeRuleSet();
            ca.ResetCurrentLine();
        }
    }
}
