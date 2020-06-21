using Godot;

/**
Example 1.6:
Normalizing a vector
*/

public class C1Example6 : Node2D, IExample
{
    public string _Summary() {
        return "Example 1.6:\nNormalizing a vector";
    }

    public override void _Draw() {
        var mousePosition = GetViewport().GetMousePosition();
        var size = GetViewport().Size;
        var center = size / 2;

        var target = (mousePosition - center).Normalized();

        DrawLine(center, center + target * 100, Colors.Black, 2, true);
    }

    public override void _Process(float delta) {
        Update();
    }
}
