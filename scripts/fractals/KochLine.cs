using Godot;
using System.Collections.Generic;

/// <summary>
/// Fractal objects.
/// </summary>
namespace Fractals
{
    /// <summary>
    /// Koch line.
    /// </summary>
    public class KochLine : Resource
    {
        /// <summary>Start point</summary>
        public Vector2 Start;
        /// <summary>End point</summary>
        public Vector2 End;

        /// <summary>
        /// Draw line using canvas.
        /// </summary>
        /// <param name="canvas">Canvas item</param>
        public void Draw(CanvasItem canvas)
        {
            canvas.DrawLine(Start, End, Colors.White);
        }

        /// <summary>Get point A.</summary>
        /// <returns>Point.</returns>
        public Vector2 KochA()
        {
            return Start;
        }

        /// <summary>Get point B.</summary>
        /// <returns>Point.</returns>
        public Vector2 KochB()
        {
            return ((End - Start) / 3) + Start;
        }

        /// <summary>Get point C.</summary>
        /// <returns>Point.</returns>
        public Vector2 KochC()
        {
            var o = Start;
            var v = (End - Start) / 3;
            return o + v + v.Rotated(-Mathf.Deg2Rad(60));
        }

        /// <summary>Get point D.</summary>
        /// <returns>Point.</returns>
        public Vector2 KochD()
        {
            return ((End - Start) * 2 / 3.0f) + Start;
        }

        /// <summary>Get point E.</summary>
        /// <returns>Point.</returns>
        public Vector2 KochE()
        {
            return End;
        }
    }

    /// <summary>
    /// Koch curve.
    /// </summary>
    public class KochCurve : Resource
    {
        private readonly int _generations;

        private List<KochLine> _lines = new List<KochLine>();

        /// <summary>Line count.</summary>
        public int Count => _lines.Count;

        /// <summary>
        /// Create new Koch curve.
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">Ending point</param>
        /// <param name="generations">Generation count</param>
        public KochCurve(Vector2 start, Vector2 end, int generations)
        {
            _generations = generations;

            _lines.Add(new KochLine()
            {
                Start = start,
                End = end
            });
        }

        /// <summary>
        /// Generate all needed generations.
        /// </summary>
        public void GenerateAll()
        {
            for (int i = 0; i < _generations; ++i)
                GenerateOne();
        }

        /// <summary>
        /// Generate one generation.
        /// </summary>
        public void GenerateOne()
        {
            var next = new List<KochLine>();

            foreach (var line in _lines)
            {
                var a = line.KochA();
                var b = line.KochB();
                var c = line.KochC();
                var d = line.KochD();
                var e = line.KochE();

                next.Add(new KochLine()
                {
                    Start = a,
                    End = b
                });
                next.Add(new KochLine()
                {
                    Start = b,
                    End = c
                });
                next.Add(new KochLine()
                {
                    Start = c,
                    End = d
                });
                next.Add(new KochLine()
                {
                    Start = d,
                    End = e
                });
            }

            _lines = next;
        }

        /// <summary>
        /// Draw curve using canvas.
        /// </summary>
        /// <param name="canvas">Canvas item</param>
        public void Draw(CanvasItem canvas)
        {
            foreach (var line in _lines)
                line.Draw(canvas);
        }

        /// <summary>
        /// Draw curve using canvas until line index.
        /// </summary>
        /// <param name="canvas">Canvas item</param>
        /// <param name="index">Line index</param>
        public void DrawUntil(CanvasItem canvas, int index)
        {
            var limit = Mathf.Max(0, Mathf.Min(index, Count));
            for (var i = 0; i < limit; ++i)
                _lines[i].Draw(canvas);
        }
    }

    /// <summary>
    /// Koch curve node.
    /// </summary>
    public class KochCurveNode : Node2D
    {
        /// <summary>Animate curve</summary>
        public bool Animated;
        /// <summary>Koch curve</summary>
        public KochCurve KochCurve;

        private int _currentLineIdx;
        private int _curveSize;

        public override void _Ready()
        {
            KochCurve.GenerateAll();
            _curveSize = KochCurve.Count;
        }

        public override void _Draw()
        {
            if (!Animated)
            {
                KochCurve.Draw(this);
            }
            else
            {
                KochCurve.DrawUntil(this, _currentLineIdx);
            }
        }

        public override void _Process(float delta)
        {
            if (Animated)
            {
                Update();
                _currentLineIdx = (_currentLineIdx + 4) % _curveSize;
            }
        }
    }
}
