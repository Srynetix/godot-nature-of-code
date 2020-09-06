using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.14: Vehicle Force Factors.
  /// </summary>
  public class C6Exercise14 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 6.14:\nVehicle Force Factors\n\nTouch screen to spawn vehicles";
    }

    private const int vehicleCount = 50;
    private readonly List<SimpleVehicle> vehicles = new List<SimpleVehicle>();

    private class RandomVehicle : RoundVehicle
    {
      private float t = 0;

      public override void _Process(float delta)
      {
        t += delta * 4;

        // Force factors depends on time
        SeparationForceFactor = MathUtils.Map(Mathf.Sin(t), -1, 1, 0.5f, 1.5f);
        SeekForceFactor = 2 - SeparationForceFactor;

        while (t > Mathf.Pi * 2)
        {
          t -= Mathf.Pi * 2;
        }

        base._Process(delta);
      }
    }

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
          var vehicle = new RandomVehicle
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
