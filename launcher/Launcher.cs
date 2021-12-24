using Godot;

/// <summary>
/// Main launcher.
/// </summary>
public class Launcher : Control
{
    /// <summary>Current app version</summary>
    public const string VERSION = "1.2.0";

    /// <summary>Default clear color</summary>
    public static Color DefaultClearColor = Color.Color8(45, 45, 45);

    private VBoxContainer _launcherUI;
    private ColorRect _background;
    private Button _backButton;
    private Button _examplesButton;
    private Button _ecosystemButton;
    private Button _quitButton;
    private Label _fpsLabel;
    private Label _versionLabel;
    private RichTextLabel _links;
    private Control _drawSpace;

    public override void _Ready()
    {
        // Base default clear color
        VisualServer.SetDefaultClearColor(DefaultClearColor);

        _background = GetNode<ColorRect>("Background");
        _launcherUI = GetNode<VBoxContainer>("Margin/VBox");
        _backButton = GetNode<Button>("Margin/BackButton");
        _examplesButton = GetNode<Button>("Margin/VBox/Margin/Buttons/ExamplesButton");
        _ecosystemButton = GetNode<Button>("Margin/VBox/Margin/Buttons/EcosystemButton");
        _quitButton = GetNode<Button>("Margin/VBox/Margin/Buttons/QuitButton");
        _links = GetNode<RichTextLabel>("Margin/VBox/Margin/Links");
        _drawSpace = GetNode<Control>("DrawSpace");
        _fpsLabel = GetNode<Label>("Margin/FPS");
        _versionLabel = GetNode<Label>("Margin/Version");

        _examplesButton.Connect("pressed", this, nameof(LoadSceneExplorer));
        _ecosystemButton.Connect("pressed", this, nameof(LoadEcosystem));
        _quitButton.Connect("pressed", this, nameof(Quit));
        _backButton.Connect("pressed", this, nameof(ReloadLauncher));
        _links.Connect("meta_clicked", this, nameof(LinkClicked));

        // Set version
        _versionLabel.Text = "Version " + VERSION;

        if (OS.GetName() == "HTML5")
        {
            _quitButton.Hide();
        }

        ToggleBackUI(false);
    }

    public override void _Process(float delta)
    {
        _fpsLabel.Text = "FPS: " + Engine.GetFramesPerSecond();
    }

    private void LoadSceneExplorer()
    {
        ToggleLauncherUI(false);

        var sceneExplorer = (PackedScene)GD.Load("res://chapters/SceneExplorer.tscn");
        _drawSpace.AddChild(sceneExplorer.Instance());

        ToggleBackUI(true);
    }

    private void LoadEcosystem()
    {
        var ecosystem = (PackedScene)GD.Load("res://ecosystem/Ecosystem.tscn");
        _drawSpace.AddChild(ecosystem.Instance());

        ToggleLauncherUI(false);
        ToggleBackUI(true);
    }

    private void ToggleLauncherUI(bool state)
    {
        _versionLabel.Visible = state;
        _launcherUI.Visible = state;
        _background.Visible = state;
    }

    private void ToggleBackUI(bool state)
    {
        _backButton.Visible = state;
    }

    private void LinkClicked(object data)
    {
        if (data is string stringData)
        {
            OS.ShellOpen(stringData);
        }
    }

    private void ReloadLauncher()
    {
        GetTree().ReloadCurrentScene();
    }

    private void Quit()
    {
        GetTree().Quit();
    }
}
