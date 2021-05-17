using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise10 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.10:\nPerlin tree";
            }

            private float _t;
            private readonly OpenSimplexNoise _noise = new OpenSimplexNoise();

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawTree(new Vector2(size.x / 2, size.y / 1.15f), 0, 125);
            }

            public override void _Process(float delta)
            {
                _t += delta * 50;

                Update();

                // Wrap value
                _t = Mathf.PosMod(_t, 100_000);
            }

            private void DrawTree(Vector2 position, float rotation, float length)
            {
                if (length <= 2)
                {
                    return;
                }

                var start = position;
                var end = position + new Vector2(0, -length).Rotated(rotation);
                var newLength = length * 0.66f;
                var windValue = MathUtils.Map(_noise.GetNoise1d(_t), 0, 1, -0.05f, 0.05f);
                const float newRotation = Mathf.Pi / 6;

                DrawLine(start, end, Colors.White);
                DrawTree(end, rotation + newRotation + windValue, newLength);
                DrawTree(end, rotation - newRotation + windValue, newLength);
            }
        }
    }
}
