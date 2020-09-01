using Godot;

namespace Examples
{
  namespace Chapter0
  {
    /// <summary>
    /// Example 0.2 - Random number distribution.
    /// </summary>
    /// Uses the Node2D _Draw function to draw bars.
    public class C0Example2 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example I.2:\n"
          + "Random number distribution";
      }

      private int[] randomCounts;

      public override void _Ready()
      {
        GD.Randomize();
        randomCounts = new int[20];
      }

      public override void _Draw()
      {
        var index = (int)GD.RandRange(0, randomCounts.Length);
        randomCounts[index] += 10;

        var color = Colors.LightGray;
        var width = GetViewportRect().Size.x;
        var height = GetViewportRect().Size.y;
        var w = width / (float)randomCounts.Length;

        for (int x = 0; x < randomCounts.Length; x++)
        {
          var rect = new Rect2(x * w, height - randomCounts[x], w - 1, randomCounts[x]);
          DrawRect(rect, color);
        }
      }

      public override void _Process(float delta)
      {
        Update();
      }
    }
  }
}
