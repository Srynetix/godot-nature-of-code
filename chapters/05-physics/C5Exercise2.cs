using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Exercise 5.2 - Anti-Gravity Boxes.
    /// </summary>
    /// Custom SimpleBox without gravity.
    public class C5Exercise2 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 5.2:\n"
              + "Anti-Gravity Boxes\n\n"
              + "Touch screen to spawn boxes";
        }

        private class AntigravityBox : SimpleBox
        {
            public override void _Ready()
            {
                base._Ready();

                // Remove gravity
                GravityScale = 0;
            }
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var spawner = new SimpleTouchSpawner()
            {
                SpawnFunction = (position) =>
                {
                    return new AntigravityBox()
                    {
                        BodySize = new Vector2(20, 20),
                        GlobalPosition = position
                    };
                }
            };
            AddChild(spawner);

            const int boxCount = 10;
            for (int i = 0; i < boxCount; ++i)
            {
                spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
            }
        }
    }
}
