using Godot;

/// <summary>
/// Boot screen.
/// </summary>
public class BootScreen : Control
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Connect("animation_finished", this, nameof(LoadLauncher));
    }

    private void LoadLauncher(string _1)
    {
        GetTree().ChangeScene("res://launcher/Launcher.tscn");
    }
}
