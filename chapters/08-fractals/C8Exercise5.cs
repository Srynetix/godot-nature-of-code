using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise5 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.5:\nSierpinski triangle";
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                Sierpinski(size / 2, size.y / 2.5f, 0);
            }

            private void Sierpinski(Vector2 position, float diameter, int step)
            {
                var a = position + new Vector2(0, -diameter);
                var b = a + new Vector2(0, diameter * 2).Rotated(Mathf.Deg2Rad(30));
                var c = a + new Vector2(0, diameter * 2).Rotated(-Mathf.Deg2Rad(30));

                if (diameter > 5)
                {
                    var newA = position - new Vector2(0, diameter / 2);
                    var newB = newA + new Vector2(0, diameter).Rotated(Mathf.Deg2Rad(30));
                    var newC = newA + new Vector2(0, diameter).Rotated(Mathf.Deg2Rad(-30));
                    Sierpinski(newA, diameter / 2, step + 1);
                    Sierpinski(newB, diameter / 2, step + 1);
                    Sierpinski(newC, diameter / 2, step + 1);
                }
                else
                {
                    DrawPolygon(new Vector2[] { a, b, c }, new Color[] { Colors.White, Colors.White, Colors.White });
                }
            }
        }
    }
}
