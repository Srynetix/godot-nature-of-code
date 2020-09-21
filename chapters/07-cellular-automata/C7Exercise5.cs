using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.5: All RuleSets.
    /// </summary>
    public class C7Exercise5 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.5:\nAll RuleSets";
        }

        private CellularAutomata1D ca;

        public override void _Ready()
        {
            ca = new CellularAutomata1D();
            AddChild(ca);

            ca.RuleNumber = 0;
            ca.WaitTime = 0.016f;
            ca.Connect(nameof(CellularAutomata1D.ScreenCompleted), this, nameof(SetRandomRule));
        }

        private void SetRandomRule()
        {
            ca.RuleNumber = (byte)((ca.RuleNumber + 1) % 255);
            ca.ResetCurrentLine();
        }
    }
}
