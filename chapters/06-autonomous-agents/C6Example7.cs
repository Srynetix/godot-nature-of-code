using Godot;
using Agents;
using Physics;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.7: Group Separation.
  /// </summary>
  /// Use SimpleVehicle.Separate method.
  public class C6Example7 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Example 6.7:\nGroup Separation\n\nTouch screen to spawn vehicles";
    }

    private int vehicleCount = 50;
    private List<SimpleVehicle> vehicles = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var spawner = new SimpleTouchSpawner();
      spawner.SpawnFunction = (pos) =>
      {
        var vehicle = new SimpleVehicle();
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

    public override void _Process(float delta)
    {
      foreach (var vehicle in vehicles)
      {
        vehicle.Separate(vehicles);
      }
    }
  }
}
