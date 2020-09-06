using Godot;
using Agents;
using System.Collections.Generic;
using Utils;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.9: Flocking.
  /// </summary>
  public class C6Example9 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 6.9:\nFlocking\n\nTouch screen to spawn boids";
    }

    private readonly List<SimpleVehicle> boids = new List<SimpleVehicle>();

    public override void _Ready()
    {
      const int boidsCount = 50;
      var size = GetViewportRect().Size;
      var spawner = new SimpleTouchSpawner
      {
        SpawnFunction = (pos) =>
        {
          var boid = new SimpleBoid
          {
            VehicleGroupList = boids,
            Position = pos
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
