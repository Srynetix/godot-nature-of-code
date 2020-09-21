using Godot;
using Agents;
using Utils;
using System.Collections.Generic;

namespace Examples.Chapter6
{
    /// <summary>
    /// Exercise 6.17: Flocking with Lateral Move.
    /// </summary>
    public class C6Exercise17 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 6.17:\nFlocking with Lateral Move";
        }

        private class LateralMovingBoid : SimpleBoid
        {
            public LateralMovingBoid()
            {
                LateralMoveEnabled = true;
                LateralMoveForceFactor = 0.75f;
            }
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
                    var boid = new LateralMovingBoid
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
