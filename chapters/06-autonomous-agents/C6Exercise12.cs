using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.12: Group Cohesion.
  /// </summary>
  public class C6Exercise12 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.12:\nGroup Cohesion\n\nTouch screen to spawn vehicles";
    }

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
        vehicle.CohesionEnabled = true;
        vehicle.Position = pos;
        vehicles.Add(vehicle);
        return vehicle;
      };
      AddChild(spawner);

      for (int i = 0; i < vehicleCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
      }
    }
  }
}
