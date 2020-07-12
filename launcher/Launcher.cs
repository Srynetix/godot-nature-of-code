using Godot;

public class Launcher : Control {
  private VBoxContainer launcherUI;
  private ColorRect background;
  private Button backButton;
  private Button examplesButton;
  private Button ecosystemButton;
  private Button quitButton;
  private Label fpsLabel;
  private RichTextLabel links;
  private Control drawSpace;

  public override void _Ready() {
    background = GetNode<ColorRect>("Background");
    launcherUI = GetNode<VBoxContainer>("Margin/VBox");
    backButton = GetNode<Button>("Margin/BackButton");
    examplesButton = GetNode<Button>("Margin/VBox/Margin/Buttons/ExamplesButton");
    ecosystemButton = GetNode<Button>("Margin/VBox/Margin/Buttons/EcosystemButton");
    quitButton = GetNode<Button>("Margin/VBox/Margin/Buttons/QuitButton");
    links = GetNode<RichTextLabel>("Margin/VBox/Margin/Links");
    drawSpace = GetNode<Control>("DrawSpace");
    fpsLabel = GetNode<Label>("Margin/FPS");

    examplesButton.Connect("pressed", this, nameof(LoadSceneExplorer));
    ecosystemButton.Connect("pressed", this, nameof(LoadEcosystem));
    quitButton.Connect("pressed", this, nameof(Quit));
    backButton.Connect("pressed", this, nameof(ReloadLauncher));
    links.Connect("meta_clicked", this, nameof(LinkClicked));

    ToggleBackUI(false);
  }

  private void LoadSceneExplorer() {
    var sceneExplorer = (PackedScene)GD.Load("res://chapters/SceneExplorer.tscn");
    drawSpace.AddChild(sceneExplorer.Instance());

    ToggleLauncherUI(false);
    ToggleBackUI(true);
  }

  private void LoadEcosystem() {
    var ecosystem = (PackedScene)GD.Load("res://ecosystem/Ecosystem.tscn");
    drawSpace.AddChild(ecosystem.Instance());

    ToggleLauncherUI(false);
    ToggleBackUI(true);
  }

  private void ToggleLauncherUI(bool state) {
    launcherUI.Visible = state;
    background.Visible = state;
  }

  private void ToggleBackUI(bool state) {
    backButton.Visible = state;
  }

  private void LinkClicked(object data) {
    if (data is string stringData) {
      OS.ShellOpen(stringData);
    }
  }

  private void ReloadLauncher() {
    GetTree().ReloadCurrentScene();
  }

  private void Quit() {
    GetTree().Quit();
  }

  public override void _Process(float delta) {
    fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
  }
}
