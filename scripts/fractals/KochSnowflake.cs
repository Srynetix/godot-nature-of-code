using Godot;

namespace Fractals
{
    public class KochSnowflake: Node2D
    {
        public float Diameter;
        public int Generations;

        private KochCurve _curveA;
        private KochCurve _curveB;
        private KochCurve _curveC;

        public override void _Ready()
        {
            CreatePoints();
        }

        private void CreatePoints() {
            var pointA = new Vector2(0, -Diameter);
            var pointB = pointA + new Vector2(0, Diameter * 2).Rotated(Mathf.Deg2Rad(30));
            var pointC = pointA + new Vector2(0, Diameter * 2).Rotated(-Mathf.Deg2Rad(30));

            _curveA = new KochCurve(pointB, pointA, Generations);
            _curveB = new KochCurve(pointC, pointB, Generations);
            _curveC = new KochCurve(pointA, pointC, Generations);

            _curveA.GenerateAll();
            _curveB.GenerateAll();
            _curveC.GenerateAll();
        }

        public override void _Draw() {
            _curveA.Draw(this);
            _curveB.Draw(this);
            _curveC.Draw(this);
        }
    }
}
