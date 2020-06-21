using Godot;

/**
Example 1.3:
Vector subtraction
*/

public class C1Example3 : Node2D, IExample
{
    public string _Summary() {
        return "Example 1.3:\nVector subtraction";
    }

    public override void _Draw() {
        var mousePosition = GetViewport().GetMousePosition();
        var size = GetViewport().Size;
        var center = size / 2;

        var target = mousePosition - center;

        DrawLine(center, center + target, Colors.Black, 2, true);
    }

    public override void _Process(float delta) {
        Update();
    }
}
