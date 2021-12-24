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

            private Font _defaultFont;
            private string _current = "A";
            private int _generation;

            public override void _Ready()
            {
                _defaultFont = SimpleDefaultFont.Regular;
            }

            private string GenerateOne()
            {
                var builder = new StringBuilder();

                for (int i = 0; i < _current.Length; ++i)
                {
                    char currentChar = _current[i];
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
                if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && _generation < MAX_GENERATION)
                {
                    _current = GenerateOne();
                    _generation++;
                    Update();
                }
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawString(_defaultFont, new Vector2(10, size.y / 2), "Generation: " + _generation);
                DrawString(_defaultFont, new Vector2(10, (size.y / 2) + 16), "Value: " + _current);
                DrawString(_defaultFont, new Vector2(10, (size.y / 2) + 32), "Length: " + _current.Length);
            }
        }
    }
}
