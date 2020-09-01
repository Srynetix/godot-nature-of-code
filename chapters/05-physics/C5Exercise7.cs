using Godot;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Exercise 5.7 - Car Simulation.
    /// </summary>
    /// Uses SimpleBall with torque to drive the car.
    /// Reuses the WaveWall from Exercise 5.3.
    public class C5Exercise7 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 5.7:\n"
          + "Car Simulation\n\n"
          + "Touch screen to spawn balls";
      }

      public class Wheel : Physics.SimpleBall
      {
        public float Torque = 20f;

        public Wheel()
        {
          Mass = 10f;
        }

        public override void _PhysicsProcess(float delta)
        {
          ApplyTorqueImpulse(Torque);
        }
      }

      public class CarBase : Physics.SimpleBox
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
          carLeftWheel.Position = carBase.Position + new Vector2(-carBase.BodySize.x / 2 + carLeftWheel.Radius, carBase.BodySize.y);
          AddChild(carLeftWheel);
          var leftJoint = new PinJoint2D();
          leftJoint.NodeA = carBase.GetPath();
          leftJoint.NodeB = carLeftWheel.GetPath();
          leftJoint.Softness = 0;
          carLeftWheel.AddChild(leftJoint);

          var carRightWheel = new Wheel();
          carRightWheel.Position = carBase.Position + new Vector2(carBase.BodySize.x / 2 - carRightWheel.Radius, carBase.BodySize.y);
          AddChild(carRightWheel);
          var rightJoint = new PinJoint2D();
          rightJoint.NodeA = carBase.GetPath();
          rightJoint.NodeB = carRightWheel.GetPath();
          rightJoint.Softness = 0;
          carRightWheel.AddChild(rightJoint);
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        var wall = new C5Exercise3.WaveWall();
        wall.Length = size.x;
        wall.Frequency = 0.1f;
        wall.Amplitude = 100;
        wall.Position = new Vector2(size.x / 2, size.y - 200);
        AddChild(wall);

        var car = new Car();
        car.Position = new Vector2(size.x / 8, size.y / 2);
        AddChild(car);

        var spawner = new Physics.SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var body = new Physics.SimpleBall();
          body.Radius = 5;
          body.Mass = 0.00001f;
          body.GlobalPosition = position;
          return body;
        };
        AddChild(spawner);
      }
    }
  }
}
