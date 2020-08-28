using Godot;
using System.Linq;
using Oscillation;

public class C3Example7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 3.7:\n"
      + "Oscillators";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    int oscillatorsCount = 10;

    foreach (int i in Enumerable.Range(0, oscillatorsCount))
    {
      var oscillator = new SimpleOscillator();
      oscillator.Amplitude = new Vector2((float)GD.RandRange(0, size.x / 2), (float)GD.RandRange(0, size.y / 2));
      oscillator.Position = size / 2;
      AddChild(oscillator);
    }
  }
}
