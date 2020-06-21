using Godot;

/**
Example 1.4:
Vector multiplication
*/

public class C1Example4 : Node2D, IExample
{
    public string _Summary()
    {
        return "Example 1.4:\nVector multiplication";
    }

    public override void _Draw()
    {
        var mousePosition = GetViewport().GetMousePosition();
        var size = GetViewport().Size;
        var center = size / 2;

        var target = (mousePosition - center) * 0.5f;

        DrawLine(center, center + target, Colors.Black, 2, true);
    }

    public override void _Process(float delta)
    {
        Update();
    }
}
