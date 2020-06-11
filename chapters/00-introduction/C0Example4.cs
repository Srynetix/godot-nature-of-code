using Godot;

/**
Example I.4:
Gaussian distribution
*/

public class C0Example4 : Node2D {
    private RandomNumberGenerator generator;

    public override void _Ready() {
        generator = new RandomNumberGenerator();
        generator.Randomize();

        VisualServer.SetDefaultClearColor(Colors.White);
        GetViewport().RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
    }

    public override void _Draw() {
        var size = GetViewport().Size;

        float num = generator.Randfn(0, 1);  // Gaussian distribution
        float sd = size.x / 8;
        float mean = size.x / 2;

        float x = sd * num + mean;

        DrawCircle(new Vector2(x, size.y / 2), 8, Color.Color8(0, 0, 0, 10));
    }

    public override void _Process(float delta) {
        Update();
    }
}
