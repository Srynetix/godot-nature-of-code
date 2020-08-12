using Godot;
using System.Linq;

public class C3Exercise8 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 3.8:\n"
      + "Oscillators accel.";
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    int oscillatorsCount = 10;
    var sizeOffset = size.x / oscillatorsCount;

    foreach (int i in Enumerable.Range(0, oscillatorsCount))
    {
      var oscillator = new SimpleOscillator();
      oscillator.Velocity = Vector2.Zero;
      oscillator.Amplitude = new Vector2(24, size.y / 4);
      oscillator.AngularAcceleration = new Vector2(0.01f, 0.025f * (i + 1));
      oscillator.Position = new Vector2(sizeOffset / 2 + sizeOffset * i, size.y / 2);
      AddChild(oscillator);
    }
  }
}
