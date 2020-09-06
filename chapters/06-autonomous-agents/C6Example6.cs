using Godot;
using Agents;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.6: Path Following.
  /// </summary>
  /// Uses SimplePath.
  public class C6Example6 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 6.6:\nPath Following";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var path = new SimplePath();
      path.Points.Add(new Vector2(0, size.y / 2));
      path.Points.Add(new Vector2(size.x / 8, size.y * 3 / 4));
      path.Points.Add(new Vector2(size.x / 2, (size.y / 2) - 50));
      path.Points.Add(new Vector2(size.x, size.y / 2));
      AddChild(path);

      var vehicle1 = new SimpleVehicle {
        Position = new Vector2(100, 100),
        TargetPath = path
      };
      AddChild(vehicle1);

      var vehicle2 = new SimpleVehicle {
        Position = new Vector2(100, size.y - 100),
        TargetPath = path
      };
      AddChild(vehicle2);
    }
  }
}
