using Godot;
using System.Collections.Generic;

namespace Fractals
{
    /// <summary>
    /// Cantor line.
    /// </summary>
    public class CantorLine
    {
        /// <summary>Starting point.</summary>
        public Vector2 Start;
        /// <summary>Ending point.</summary>
        public Vector2 End;
        /// <summary>Line color.</summary>
        public Color Color = Colors.White;
        /// <summary>Line width.</summary>
        public float Width = 10;

        /// <summary>Get point A.</summary>
        /// <returns>Point.</returns>
        public Vector2 PointA()
        {
            return Start;
        }

        /// <summary>Get point B.</summary>
        /// <returns>Point.</returns>
        public Vector2 PointB()
        {
            return ((End - Start) / 3) + Start;
        }

        /// <summary>Get point C.</summary>
        /// <returns>Point.</returns>
        public Vector2 PointC()
        {
            return ((End - Start) * 2 / 3.0f) + Start;
        }

        /// <summary>Get point D.</summary>
        /// <returns>Point.</returns>
        public Vector2 PointD()
        {
            return End;
        }

        /// <summary>
        /// Draw line using canvas.
        /// </summary>
        /// <param name="canvas">Canvas item</param>
        public void Draw(CanvasItem canvas)
        {
            canvas.DrawLine(Start, End, Colors.White, Width);
        }
    }

    /// <summary>
    /// Cantor set.
    /// </summary>
    public class CantorSet
    {
        private readonly int _generations;
        private readonly float _separation;
        private readonly List<List<CantorLine>> _lines = new List<List<CantorLine>>();

        /// <summary>
        /// Create new cantor set.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="end">Ending point.</param>
        /// <param name="generations">Generation count.</param>
        /// <param name="separation">Separation.</param>
        public CantorSet(Vector2 start, Vector2 end, int generations, float separation)
        {
            _generations = generations;
            _separation = separation;

            _lines.Add(new List<CantorLine>() {
                new CantorLine() {
                    Start = start,
                    End = end
                }
            });
        }

        /// <summary>
        /// Generate one generation.
        /// </summary>
        public void GenerateOne()
        {
            var next = new List<CantorLine>();

            foreach (var line in _lines[^1])
            {
                var a = line.PointA() + new Vector2(0, _separation);
                var b = line.PointB() + new Vector2(0, _separation);
                var c = line.PointC() + new Vector2(0, _separation);
                var d = line.PointD() + new Vector2(0, _separation);

                next.Add(new CantorLine()
                {
                    Start = a,
                    End = b
                });
                next.Add(new CantorLine()
                {
                    Start = c,
                    End = d
                });
            }

            _lines.Add(next);
        }

        /// <summary>
        /// Generate all needed generations.
        /// </summary>
        public void GenerateAll()
        {
            for (var i = 0; i < _generations; ++i)
                GenerateOne();
        }

        /// <summary>
        /// Draw set using canvas.
        /// </summary>
        /// <param name="canvas">Canvas item</param>
        public void Draw(CanvasItem canvas)
        {
            foreach (var lineArray in _lines)
            {
                foreach (var line in lineArray)
                    line.Draw(canvas);
            }
        }
    }

    /// <summary>
    /// Cantor set node.
    /// </summary>
    public class CantorSetNode : Node2D
    {
        /// <summary>Cantor set.</summary>
        public CantorSet CantorSet;

        public override void _Ready()
        {
            CantorSet.GenerateAll();
        }

        public override void _Draw()
        {
            CantorSet.Draw(this);
        }
    }
}
