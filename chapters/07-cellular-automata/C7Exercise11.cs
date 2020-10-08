using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.11: Continuous Game of Life.
    /// </summary>
    /// Simulate a float using an int between 0 and 100 for state.
    public class C7Exercise11 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.11:\nContinuous Game of Life";
        }

        private class IntCell : Cell<int>
        {
            public override int RandomizeState()
            {
                return MathUtils.RandRangei(GetDeadValue(), GetAliveValue());
            }

            public override int GetAliveValue()
            {
                return 100;
            }

            public override int GetDeadValue()
            {
                return 0;
            }

            protected override Color? GetStateColor()
            {
                if (State == 0)
                {
                    return null;
                }

                return CellColor;
            }
        }

        private class ContinuousGameOfLife : CellularAutomata2D<IntCell, int>
        {
            protected override int ApplyRules(int x, int y)
            {
                var neighbors = GetAliveNeighborsFromCell(x, y);
                var state = _grid[x + (y * _cols)].State;

                if (state > 20 && (neighbors < 200 || neighbors > 400))
                {
                    return 0;
                }
                else if (state < 80 && neighbors > 200 && neighbors < 300)
                {
                    return 100;
                }

                return Mathf.Clamp(state - MathUtils.RandRangei(-1, 1), 0, 100);
            }
        }

        public override void _Ready()
        {
            var ca = new ContinuousGameOfLife
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
            };
            AddChild(ca);

            ca.RandomizeGrid();
        }
    }
}
