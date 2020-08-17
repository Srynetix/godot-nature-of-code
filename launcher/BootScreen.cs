using Godot;

public class BootScreen : Control
{
  private AnimationPlayer animationPlayer;

  public override void _Ready()
  {
    animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    animationPlayer.Connect("animation_finished", this, nameof(LoadLauncher));
  }

  private void LoadLauncher(string animationName)
  {
    GetTree().ChangeScene("res://launcher/Launcher.tscn");
  }
}
