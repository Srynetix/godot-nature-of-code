using Godot;
using Assets;
using System.Text;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example9 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.9:\nSimple L-System with mouse\n\nClick on screen to display next generation";
            }

            private const int MAX_GENERATION = 10;

            private Font defaultFont;
            private string current = "A";
            private int generation;

            public override void _Ready()
            {
                defaultFont = SimpleDefaultFont.Regular;
            }

            private string GenerateOne()
            {
                var builder = new StringBuilder();

                for (int i = 0; i < current.Length; ++i)
                {
                    char currentChar = current[i];
                    if (currentChar == 'A')
                    {
                        builder.Append("AB");
                    }
                    else if (currentChar == 'B')
                    {
                        builder.Append("A");
                    }
                }

                return builder.ToString();
            }

            public override void _UnhandledInput(InputEvent @event)
            {
                if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && generation < MAX_GENERATION)
                {
                    current = GenerateOne();
                    generation++;
                    Update();
                }
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawString(defaultFont, new Vector2(10, size.y / 2), "Generation: " + generation);
                DrawString(defaultFont, new Vector2(10, (size.y / 2) + 16), "Value: " + current);
                DrawString(defaultFont, new Vector2(10, (size.y / 2) + 32), "Length: " + current.Length);
            }
        }
    }
}
