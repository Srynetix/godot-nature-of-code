using Godot;
using System.Linq;

public class C3Exercise7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.7:\n"
      + "Controlled oscillators";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    int oscillatorsCount = 10;
    var sizeOffset = size.x / oscillatorsCount;

    foreach (int i in Enumerable.Range(0, oscillatorsCount))
    {
      var oscillator = new SimpleOscillator();
      oscillator.Amplitude = new Vector2(24, size.y / 4);
      oscillator.Velocity = new Vector2(0.01f, 0.025f * (i + 1));
      oscillator.Position = new Vector2(sizeOffset / 2 + sizeOffset * i, size.y / 2);
      AddChild(oscillator);
    }
  }
}
