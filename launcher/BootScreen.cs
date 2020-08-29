using Godot;

/// <summary>
/// Boot screen.
/// </summary>
public class BootScreen : Control
{
  private AnimationPlayer animationPlayer;

  #region Lifecycle methods

  public override void _Ready()
  {
    animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    animationPlayer.Connect("animation_finished", this, nameof(LoadLauncher));
  }

  #endregion

  #region Private methods

  private void LoadLauncher(string animationName)
  {
    GetTree().ChangeScene("res://launcher/Launcher.tscn");
  }

  #endregion
}
