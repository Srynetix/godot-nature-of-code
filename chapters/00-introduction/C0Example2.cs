using Godot;

/**
Example I.2:
Random number distribution
*/

public class C0Example2 : Node2D, IExample {
	private int[] randomCounts;

	public string _Summary() {
        return "Example I.2:\nRandom number distribution";
    }

	public override void _Ready() {
        GD.Randomize();
		randomCounts = new int[20];
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
