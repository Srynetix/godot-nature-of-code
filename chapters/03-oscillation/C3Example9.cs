using Godot;
using Oscillation;

namespace Examples.Chapter3
{
  /// <summary>
  /// Example 3.9 - The Wave.
  /// </summary>
  /// Uses SimpleWave.
  public class C3Example9 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 3.9:\n" +
        "The Wave";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var wave = new SimpleWave();
      wave.Position = size / 2;
      wave.Length = size.x;
      wave.Amplitude = size.y / 4;
      AddChild(wave);
    }
  }
}
