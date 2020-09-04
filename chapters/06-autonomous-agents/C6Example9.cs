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
    public string _Summary()
    {
      return "Example 6.9:\nFlocking\n\nTouch screen to spawn boids";
    }

    private List<SimpleVehicle> boids = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var boidsCount = 50;
      var spawner = new SimpleTouchSpawner();
      spawner.SpawnFunction = (pos) =>
      {
        var boid = new SimpleBoid();
        boid.VehicleGroupList = boids;
        boid.Position = pos;
        boids.Add(boid);
        return boid;
      };
      AddChild(spawner);

      for (int i = 0; i < boidsCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
      }
    }
  }
}
