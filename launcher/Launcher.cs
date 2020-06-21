using Godot;
using System;

public class Launcher : Control {
	private VBoxContainer launcherUI;
	private ColorRect background;
	private Button backButton;
	private Button examplesButton;
	private Button ecosystemButton;
	private Button quitButton;
	private Control drawSpace;

	public override void _Ready() {
		background = GetNode<ColorRect>("Background");
		launcherUI = GetNode<VBoxContainer>("Margin/VBoxContainer");
		backButton = GetNode<Button>("Margin/BackButton");
		examplesButton = GetNode<Button>("Margin/VBoxContainer/VBoxContainer/ExamplesButton");
		ecosystemButton = GetNode<Button>("Margin/VBoxContainer/VBoxContainer/EcosystemButton");
		quitButton = GetNode<Button>("Margin/VBoxContainer/VBoxContainer/QuitButton");
		drawSpace = GetNode<Control>("DrawSpace");

		examplesButton.Connect("pressed", this, nameof(LoadSceneExplorer));
		ecosystemButton.Connect("pressed", this, nameof(LoadEcosystem));
		quitButton.Connect("pressed", this, nameof(Quit));
		backButton.Connect("pressed", this, nameof(ReloadLauncher));

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

	private void ReloadLauncher() {
		GetTree().ReloadCurrentScene();
	}

	private void Quit() {
		GetTree().Quit();
	}
}
