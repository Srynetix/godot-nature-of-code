using Godot;

/**
Example 1.5:
Vector magnitude
*/

public class C1Example5 : Node2D
{
    public override void _Ready() {
        VisualServer.SetDefaultClearColor(Colors.White);
    }

    public override void _Draw() {
        var mousePosition = GetViewport().GetMousePosition();
        var size = GetViewport().Size;
        var center = size / 2;

        var target = mousePosition - center;
        var magnitude = target.Length();

        DrawRect(new Rect2(0, 0, magnitude, 16), Colors.Black);
        DrawLine(center, center + target, Colors.Black, 2, true);
    }

    public override void _Process(float delta) {
        Update();
    }
}
