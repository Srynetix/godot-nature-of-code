using Godot;

namespace Examples.Chapter1
{
  /// <summary>
  /// Example 1.5 - Vector magnitude.
  /// </summary>
  /// Uses _Draw to represent vector magnitude.
  public class C1Example5 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 1.5:\n"
        + "Vector magnitude";
    }

    public override void _Draw()
    {
      var mousePosition = GetViewport().GetMousePosition();
      var size = GetViewportRect().Size;
      var center = size / 2;

      var target = mousePosition - center;
      var magnitude = target.Length();

      DrawRect(new Rect2(0, 0, magnitude, 16), Colors.LightBlue);
      DrawLine(center, center + target, Colors.LightBlue, 2, true);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
