using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.18: Animated Flocking.
  /// </summary>
  public class C6Exercise18 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.18:\nAnimated Flocking";
    }

    private class RandomBoid : SimpleBoid
    {
      private OpenSimplexNoise noise = new OpenSimplexNoise();
      private float t = 0;

      public RandomBoid()
      {
        t = MathUtils.RandRangef(0, 10000);
      }

      public override void _Process(float delta)
      {
        t += delta * 10;

        // Animate fields
        MaxVelocity = MathUtils.Map(noise.GetNoise1d(t), -1, 1, 1, 10);
        MaxForce = MathUtils.Map(noise.GetNoise1d(t + 50), -1, 1, 0.1f, 1);
        AlignmentForceFactor = MathUtils.Map(noise.GetNoise1d(t + 100), -1, 1, 0.25f, 1.5f);
        SeparationForceFactor = MathUtils.Map(noise.GetNoise1d(t + 150), -1, 1, 0.25f, 1.5f);
        CohesionForceFactor = MathUtils.Map(noise.GetNoise1d(t + 200), -1, 1, 0.25f, 1.5f);

        base._Process(delta);
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
        var boid = new RandomBoid();
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
