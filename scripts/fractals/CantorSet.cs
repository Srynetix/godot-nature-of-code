using Godot;
using System.Collections.Generic;

namespace Fractals
{
    public class CantorLine : Resource
    {
        public Vector2 Start;
        public Vector2 End;
        public Color Color = Colors.White;
        public float Width = 10;

        public Vector2 PointA()
        {
            return Start;
        }

        public Vector2 PointB()
        {
            return ((End - Start) / 3) + Start;
        }

        public Vector2 PointC()
        {
            return ((End - Start) * 2 / 3.0f) + Start;
        }

        public Vector2 PointD()
        {
            return End;
        }

        public void Draw(CanvasItem canvas)
        {
            canvas.DrawLine(Start, End, Colors.White, Width);
        }
    }

    public class CantorSet : Resource
    {
        private readonly int _generations;
        private readonly float _separation;

        private readonly List<List<CantorLine>> _lines = new List<List<CantorLine>>();

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

        public void GenerateAll()
        {
            for (var i = 0; i < _generations; ++i)
                GenerateOne();
        }

        public void Draw(CanvasItem canvas)
        {
            foreach (var lineArray in _lines)
            {
                foreach (var line in lineArray)
                    line.Draw(canvas);
            }
        }
    }

    public class CantorSetNode : Node2D
    {
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
