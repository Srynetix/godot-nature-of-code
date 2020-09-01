using Godot;

namespace Examples
{
  /// <summary>
  /// Example 1.6 - Vector normalization.
  /// </summary>
  /// Uses _Draw to represent vector normalization.
  public class C1Example6 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 1.6:\n"
        + "Vector normalization";
    }

    public override void _Draw()
    {
      var mousePosition = GetViewport().GetMousePosition();
      var size = GetViewportRect().Size;
      var center = size / 2;

      var target = (mousePosition - center).Normalized();

      DrawLine(center, center + target * 100, Colors.LightBlue, 2, true);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }
}
