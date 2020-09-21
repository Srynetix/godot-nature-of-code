using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.7 - Car Simulation.
    /// </summary>
    /// Uses SimpleBall with torque to drive the car.
    /// Reuses the WaveWall from Exercise 5.3.
    public class C5Exercise7 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.7:\n"
              + "Car Simulation\n\n"
              + "Touch screen to spawn balls";
        }

        public class Wheel : SimpleBall
        {
            public float Torque = 300f;

            public Wheel()
            {
                Mass = 10f;
            }

            public override void _PhysicsProcess(float delta)
            {
                ApplyTorqueImpulse(Torque);
            }
        }

        public class CarBase : SimpleBox
        {
            public CarBase()
            {
                BodySize = new Vector2(80, 20);
            }
        }

        public class Car : Node2D
        {
            public override void _Ready()
            {
                var carBase = new CarBase();
                AddChild(carBase);

                var carLeftWheel = new Wheel();
                carLeftWheel.Position = carBase.Position + new Vector2((-carBase.BodySize.x / 2) + carLeftWheel.Radius, carBase.BodySize.y);
                AddChild(carLeftWheel);
                var leftJoint = new PinJoint2D
                {
                    NodeA = carBase.GetPath(),
                    NodeB = carLeftWheel.GetPath(),
                    Softness = 0
                };
                carLeftWheel.AddChild(leftJoint);

                var carRightWheel = new Wheel();
                carRightWheel.Position = carBase.Position + new Vector2((carBase.BodySize.x / 2) - carRightWheel.Radius, carBase.BodySize.y);
                AddChild(carRightWheel);
                var rightJoint = new PinJoint2D
                {
                    NodeA = carBase.GetPath(),
                    NodeB = carRightWheel.GetPath(),
                    Softness = 0
                };
                carRightWheel.AddChild(rightJoint);
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var wall = new C5Exercise3.WaveWall
            {
                Length = size.x,
                Frequency = 0.1f,
                Amplitude = 100,
                Position = new Vector2(size.x / 2, size.y - 200)
            };
            AddChild(wall);

            var car = new Car
            {
                Position = new Vector2(size.x / 8, size.y / 2)
            };
            AddChild(car);

            var spawner = new SimpleTouchSpawner
            {
                SpawnFunction = (position) =>
                {
                    return new SimpleBall
                    {
                        Radius = 5,
                        Mass = 0.00001f,
                        GlobalPosition = position
                    };
                }
            };
            AddChild(spawner);
        }
    }
}
