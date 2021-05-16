using Godot;
using Fractals;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise2 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.2:\nKoch snowflake";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var kochSnowflake = new KochSnowflake() {
                    Diameter = size.y / 4,
                    Generations = 4
                };
                kochSnowflake.Position = size / 2;
                AddChild(kochSnowflake);
            }
        }
    }
}
