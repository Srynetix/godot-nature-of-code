using Godot;
using Automata;

namespace Examples
{
  /// <summary>
  /// Chapter 7 - Cellular Automata.
  /// </summary>
  namespace Chapter7
  {
    /// <summary>
    /// Example 7.1: Wolfram Elementary Cellular Automata.
    /// </summary>
    public class C7Example1 : Node2D, IExample
    {
      public string GetSummary()
      {
        return "Example 7.1:\nWolfram Elementary Cellular Automata";
      }

      public override void _Ready()
      {
        var ca = new CellularAutomata1D();
        AddChild(ca);
      }
    }
  }
}
