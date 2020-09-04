using Godot;
using Agents;
using Forces;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.8: Seek and Separate.
  /// </summary>
  public class C6Example8 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 6.8:\nSeek and Separate";
    }

    private SimpleMover targetMover;
    private int vehicleCount = 50;
    private List<SimpleVehicle> vehicles = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var spawner = new SimpleTouchSpawner();
      spawner.SpawnFunction = (pos) =>
      {
        var vehicle = new RoundVehicle();
        vehicle.VehicleGroupList = vehicles;
        vehicle.SeparationEnabled = true;
        vehicle.Target = targetMover;
        vehicle.Position = pos;

        // Scale forces
        vehicle.SeparationForceFactor = 1.5f;
        vehicle.SeekForceFactor = 0.5f;

        vehicles.Add(vehicle);
        return vehicle;
      };
      AddChild(spawner);

      // Create target
      targetMover = new SimpleMover();
      targetMover.Position = size / 2;
      targetMover.Modulate = Colors.LightBlue.WithAlpha(128);
      AddChild(targetMover);

      // Create initial bodies
      for (int i = 0; i < vehicleCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
      }
    }

    public override void _Process(float delta)
    {
      targetMover.GlobalPosition = GetViewport().GetMousePosition();
    }
  }
}
