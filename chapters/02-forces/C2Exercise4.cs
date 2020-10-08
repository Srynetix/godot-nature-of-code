using System.Linq;
using Godot;
using Forces;

namespace Examples.Chapter2
{
    /// <summary>
    /// Exercise 2.4 - Friction pocket.
    /// </summary>
    /// Uses SimpleFrictionPocket capabilities to apply friction (and reverse friction) on SimpleMovers.
    public class C2Exercise4 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 2.4:\n"
              + "Friction Pockets";
        }

        private class Mover : SimpleMover
        {
            public Mover() : base(WrapModeEnum.Bounce) { }

            protected override void UpdateAcceleration()
            {
                var wind = new Vector2(0.1f, 0);
                var gravity = new Vector2(0, 0.98f);

                ApplyForce(wind);
                ApplyForce(gravity);
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var zone1 = new SimpleFrictionPocket
            {
                Coeff = 0.25f,
                Size = new Vector2(100, size.y),
                Position = new Vector2(size.x / 4, size.y / 2)
            };
            AddChild(zone1);

            var zone2 = new SimpleFrictionPocket
            {
                Coeff = -0.25f,
                Size = new Vector2(100, size.y),
                Position = new Vector2((size.x / 2) + (size.x / 4), size.y / 2)
            };
            AddChild(zone2);

            var zone3 = new SimpleFrictionPocket
            {
                Coeff = -2f,
                Size = new Vector2(10, size.y),
                Position = new Vector2(size.x / 2, size.y / 2)
            };
            AddChild(zone3);

            foreach (var _ in Enumerable.Range(0, 20))
            {
                var mover = new Mover();
                var bodySize = (float)GD.RandRange(20, 40);
                var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
                mover.MeshSize = new Vector2(bodySize, bodySize);
                mover.Mass = (float)GD.RandRange(5, 10);
                mover.Position = new Vector2(xPos, size.y / 2);
                AddChild(mover);
            }
        }
    }
}
