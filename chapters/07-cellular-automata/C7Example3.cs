using Godot;
using Automata;

namespace Examples.Chapter7
{
  /// <summary>
  /// Example 7.3: Game of Life OOP.
  /// </summary>
  public class C7Example3 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 7.3:\nGame of Life OOP";
    }

    public override void _Ready()
    {
      var ca = new CellularAutomata2D
      {
        WrapBehavior = WrapBehaviorEnum.Wrap,
        TouchBehavior = TouchBehaviorEnum.DrawCell,
        HighlightTransitions = true
      };
      AddChild(ca);

      ca.RandomizeGrid();
    }
  }
}
