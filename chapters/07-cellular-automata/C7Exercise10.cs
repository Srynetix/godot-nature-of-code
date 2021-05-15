using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.10: Probabilistic Game of Life.
    /// </summary>
    public class C7Exercise10 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.10:\nProbabilistic Game of Life";
        }

        private class ProbabilisticGameOfLife : GameOfLife
        {
            protected override bool ApplyRules(int x, int y)
            {
                var neighbors = GetAliveNeighborsFromCell(x, y);
                var state = _grid[x + (y * _cols)].State;

                // 4 or more neighbors, 80% chance of dying
                if (state && neighbors >= 4 && MathUtils.Randf() <= 0.8f)
                {
                    return false;
                }
                // 1 or fewer neighbors, 60% chance of dying
                else if (state && neighbors <= 1 && MathUtils.Randf() <= 0.6f)
                {
                    return false;
                }
                // 3 neighbors, revive.
                else if (!state && neighbors == 3)
                {
                    return true;
                }
                else
                {
                    return state;
                }
            }
        }

        public override void _Ready()
        {
            var ca = new ProbabilisticGameOfLife()
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
                HighlightTransitions = true
            };
            AddChild(ca);

            ca.RandomizeGrid();
        }
    }
}
