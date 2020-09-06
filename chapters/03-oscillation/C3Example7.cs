using Godot;
using System.Linq;
using Oscillation;

namespace Examples.Chapter3
{
  /// <summary>
  /// Example 3.7 - Oscillators.
  /// </summary>
  /// Uses SimpleOscillator.
  public class C3Example7 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 3.7:\n"
        + "Oscillators";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      const int oscillatorsCount = 10;

      foreach (int _ in Enumerable.Range(0, oscillatorsCount))
      {
        var oscillator = new SimpleOscillator
        {
          Amplitude = new Vector2((float)GD.RandRange(0, size.x / 2), (float)GD.RandRange(0, size.y / 2)),
          Position = size / 2
        };
        AddChild(oscillator);
      }
    }
  }
}
