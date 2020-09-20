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

    private class ContinuousGameOfLife : CellularAutomata2D
    {
      private class ContinuousCell : Cell
      {
        protected override Color? GetStateColor()
        {
          if (State <= 20)
          {
            // Cell already dead
            return null;
          }

          return CellColor;
        }
      }

      public override void RandomizeGrid()
      {
        _generation = 0;
        int offset = WrapBehavior == WrapBehaviorEnum.Wrap ? 0 : 1;

        for (int j = offset; j < _rows - offset; ++j)
        {
          for (int i = offset; i < _cols - offset; ++i)
          {
            var currPos = i + (j * _cols);
            var cell = _grid[currPos];
            cell.State = cell.PreviousState = MathUtils.RandRangei(0, 100);
          }
        }
      }

      protected override void ReviveCellAtScreenPos(Vector2 pos)
      {
        // Split position depending on scale
        var idx = pos / _scale;
        int offset = WrapBehavior == WrapBehaviorEnum.Wrap ? 0 : 1;
        int x = Mathf.Min(Mathf.Max(offset, (int)idx.x), _cols - 1 - offset);
        int y = Mathf.Min(Mathf.Max(offset, (int)idx.y), _rows - 1 - offset);

        var cell = _grid[x + (y * _cols)];
        cell.PreviousState = 0;
        cell.State = 100;
      }

      protected override int ApplyRules(int x, int y)
      {
        var neighbors = GetAliveNeighborsFromCell(x, y);
        var state = _grid[x + (y * _cols)].State;

        if (state > 20 && (neighbors < 200 || neighbors > 300))
        {
          return 0;
        }
        else if (state < 80 && neighbors > 200 && neighbors < 300)
        {
          return 100;
        }

        return state;
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
