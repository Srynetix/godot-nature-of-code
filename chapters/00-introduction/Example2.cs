using Godot;
using System;

public class Example2 : Node2D {
	private int[] randomCounts;

	public override void _Ready() {
		randomCounts = new int[20];
		VisualServer.SetDefaultClearColor(Colors.White);
	}

	public override void _Draw() {
		var index = (int)GD.RandRange(0, randomCounts.Length);
		randomCounts[index] += 10;

		var color = Colors.LightGray;
		var width = GetViewport().Size.x;
		var height = GetViewport().Size.y;
		var w = width / (float)randomCounts.Length;

		for (int x = 0; x < randomCounts.Length; x++) {
			var rect = new Rect2(x * w, height - randomCounts[x], w - 1, randomCounts[x]);
			DrawRect(rect, color);
		}
	}

    public override void _Process(float delta) {
        Update();
    }
}
