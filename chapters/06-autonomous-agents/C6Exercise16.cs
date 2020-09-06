using Godot;
using Agents;
using System.Collections.Generic;
using Utils;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.16: Flocking with Path Following.
  /// </summary>
  public class C6Exercise16 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 6.16:\nFlocking with Path Following";
    }

    private readonly List<SimpleVehicle> boids = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var path = new SimplePath();
      path.Points.Add(new Vector2(0, size.y / 2));
      path.Points.Add(new Vector2(size.x / 8, size.y * 3 / 4));
      path.Points.Add(new Vector2(size.x / 2, (size.y / 2) - 50));
      path.Points.Add(new Vector2(size.x, size.y / 2));
      AddChild(path);

      const int boidsCount = 50;
      var spawner = new SimpleTouchSpawner {
        SpawnFunction = (pos) =>
        {
          var boid = new SimpleBoid {
            VehicleGroupList = boids,
            Position = pos,
            TargetPath = path
          };
          boids.Add(boid);
          return boid;
        }
      };
      AddChild(spawner);

      for (int i = 0; i < boidsCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
      }
    }
  }
}
