using Godot;
using Agents;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.5: Simple Path Following.
  /// </summary>
  public class C6Example5 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 6.5:\nSimple Path Following";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var path = new SimplePath();
      path.Start = new Vector2(0, size.y / 3);
      path.Start = new Vector2(size.x, 2 * size.y / 3);
      AddChild(path);

      var vehicle = new SimpleVehicle();
      vehicle.TargetPath = path;
      vehicle.Position = new Vector2(100, size.y / 4);
      AddChild(vehicle);
    }
  }
}
