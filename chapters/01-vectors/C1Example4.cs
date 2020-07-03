using Godot;

public class C1Example4 : Node2D, IExample {
  public string _Summary() {
    return "Example 1.4:\n"
      + "Vector multiplication";
  }

  public override void _Draw() {
    var mousePosition = GetViewport().GetMousePosition();
    var size = GetViewport().Size;
    var center = size / 2;

    var target = (mousePosition - center) * 0.5f;

    DrawLine(center, center + target, Colors.LightBlue, 2, true);
  }

  public override void _Process(float delta) {
    Update();
  }
}
