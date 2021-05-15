using Godot;
using Drawing;
using Forces;

namespace Examples.Chapter3
{
    /// <summary>
    /// Exercise 3.3 - Car Drive.
    /// </summary>
    /// Simple drivable car using arrow keys or VirtualControls.
    public class C3Exercise3 : Control, IExample
    {
        public string GetSummary()
        {
            return "Exercise 3.3:\n"
              + "Car Drive\n\n"
              + "On desktop, use arrow keys (left arrow accelerates the car to the left, right to the right).\n"
              + "On mobile, you can use the virtual controls.";
        }

        private class Car : SimpleMover
        {
            public Car() : base(WrapModeEnum.Wrap)
            {
                Mesh.MeshType = SimpleMesh.TypeEnum.Custom;
                Mesh.CustomDrawMethod = (pen) =>
                {
                    // Car body
                    var bodyLength = Radius * 2;
                    var bodyWidth = Radius;
                    var wheelRadius = Radius / 4;

                    pen.DrawCircle(new Vector2((-bodyLength / 2) + (bodyLength / 4), -bodyWidth / 2), wheelRadius, Colors.Green);
                    pen.DrawCircle(new Vector2((bodyLength / 2) - (bodyLength / 4), -bodyWidth / 2), wheelRadius, Colors.Green);
                    pen.DrawCircle(new Vector2((-bodyLength / 2) + (bodyLength / 4), bodyWidth / 2), wheelRadius, Colors.Green);
                    pen.DrawCircle(new Vector2((bodyLength / 2) - (bodyLength / 4), bodyWidth / 2), wheelRadius, Colors.Green);

                    pen.DrawRect(new Rect2(-bodyLength / 2, -bodyWidth / 2, bodyLength, bodyWidth), Colors.LightBlue);
                    pen.DrawRect(new Rect2((-bodyLength / 2) + 2, (-bodyWidth / 2) + 2, bodyLength - 4, bodyWidth - 4), Colors.White);

                    // Front
                    pen.DrawRect(new Rect2(bodyLength / 2, -bodyWidth / 4, 2, bodyWidth / 2), Colors.Green);
                };
            }

            public void AccelerateAndTurn(float turnAmount, float accelAmount)
            {
                Acceleration = Vector2.Right.Rotated(Rotation + turnAmount) * accelAmount;
            }

            protected override void UpdateAcceleration()
            {
                ApplyFriction(0.15f);
            }

            public override void _Process(float delta)
            {
                base._Process(delta);

                if (Velocity.LengthSquared() > 0.01)
                {
                    Rotation = (float)Mathf.Atan2(Velocity.y, Velocity.x);
                }
            }
        }

        private VirtualControls controls;
        private Car car;

        public override void _Ready()
        {
            // Add virtual controls
            controls = new VirtualControls();
            AddChild(controls);

            // Add car
            car = new Car()
            {
                Position = GetViewportRect().Size / 2
            };
            AddChild(car);
        }

        public override void _Process(float delta)
        {
            if (controls.JoystickOutput.y < -0.5f || Input.IsActionPressed("ui_up"))
            {
                car.AccelerateAndTurn(0, 0.5f);
            }
            if (controls.JoystickOutput.x < -0.5f || Input.IsActionPressed("ui_left"))
            {
                car.AccelerateAndTurn(-0.5f, 0.5f);
            }
            if (controls.JoystickOutput.x > 0.5f || Input.IsActionPressed("ui_right"))
            {
                car.AccelerateAndTurn(0.5f, 0.5f);
            }
        }
    }
}
