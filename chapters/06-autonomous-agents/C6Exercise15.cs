using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.15: Boids with peripheral vision.
  /// </summary>
  public class C6Exercise15 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.15:\nBoids with peripheral vision\n\nTouch screen to spawn boids";
    }

    private class PeripheralVisionBoid : SimpleBoid
    {
      protected override Vector2 Align(List<SimpleVehicle> vehicles)
      {
        var sum = Vector2.Zero;
        int count = 0;

        // Use an arbitrary target position on the radius circle
        var detectedPos = GlobalPosition + new Vector2(DetectionAlignmentRadius, 0).Rotated(GlobalRotation);

        foreach (var vehicle in vehicles)
        {
          var dp = detectedPos.DistanceTo(vehicle.GlobalPosition);

          // Compare the distance between the target position and the neighbor
          if (dp > 0 && dp < DetectionAlignmentRadius / 2)
          {
            sum += vehicle.Velocity;
            count++;
          }
        }

        if (count > 0)
        {
          sum = (sum / count).Normalized() * MaxVelocity;
          return (sum - Velocity).Clamped(MaxForce);
        }
        else
        {
          return Vector2.Zero;
        }
      }
    }

    private List<SimpleVehicle> boids = new List<SimpleVehicle>();

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var boidsCount = 50;
      var spawner = new SimpleTouchSpawner();
      spawner.SpawnFunction = (pos) =>
      {
        var boid = new PeripheralVisionBoid();
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
