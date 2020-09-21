using System.Linq;
using Godot;
using Drawing;
using Forces;

namespace Examples.Chapter3
{
    /// <summary>
    /// Example 3.2 - Forces with angular motion.
    /// </summary>
    /// Uses angular motion capabilities of SimpleMovers.
    public class C3Example2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 3.2:\n"
              + "Forces with angular motion";
        }

        private class Square : SimpleMover
        {
            public Square() : base(WrapModeEnum.Wrap)
            {
                MaxVelocity = 2.0f;
                Mesh.MeshType = SimpleMesh.TypeEnum.Square;
            }

            protected override void UpdateAcceleration()
            {
                Acceleration += new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1)) * 0.02f;
                AngularAcceleration = Acceleration.x / 10.0f;
            }
        }

        public override void _Draw()
        {
            var size = GetViewportRect().Size;
            DrawCircle(size / 2, 20, Colors.LightBlue);
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            // Spawn squares
            const int squareCount = 20;
            foreach (int _ in Enumerable.Range(0, squareCount))
            {
                var square = new Square();
                var radius = (float)GD.RandRange(20, 40);
                square.MeshSize = new Vector2(radius, radius);
                square.Position = MathUtils.RandVector2(size.x / 8, size.x / 1.125f, size.y / 8, size.y / 1.125f);
                AddChild(square);
            }
        }
    }
}
