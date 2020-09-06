using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.13: Crowd Path Following.
  /// </summary>
  /// Combines SimpleVehicle.SeparationEnabled and path following.
  public class C6Exercise13 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 6.13:\nCrowd Path Following\n\nTouch screen to spawn vehicles";
    }

    private const int vehicleCount = 50;
    private readonly List<SimpleVehicle> vehicles = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var path = new SimplePath();
      path.Points.Add(new Vector2(size.x * 1 / 4, size.y * 1 / 4));
      path.Points.Add(new Vector2(size.x * 3 / 4, size.y * 1 / 4));
      path.Points.Add(new Vector2(size.x * 3 / 4, size.y * 3 / 4));
      path.Points.Add(new Vector2((size.x * 1 / 4) - 20, size.y * 3 / 4));
      path.Looping = true;
      AddChild(path);

      var spawner = new SimpleTouchSpawner
      {
        SpawnFunction = (pos) =>
        {
          var vehicle = new RoundVehicle
          {
            VehicleGroupList = vehicles,
            Position = pos,
            TargetPath = path,
            SeparationEnabled = true
          };
          vehicles.Add(vehicle);
          return vehicle;
        }
      };
      AddChild(spawner);

      for (int i = 0; i < vehicleCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
      }
    }
  }
}
