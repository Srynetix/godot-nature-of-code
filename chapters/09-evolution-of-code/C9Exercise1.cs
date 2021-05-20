using Godot;
using System.Text;

namespace Examples
{
    /// <summary>
    /// Chapter 9 - Evolution of code.
    /// </summary>
    namespace Chapter9
    {
        /// <summary>
        /// Exercise 9.1: Random string generation.
        /// </summary>
        public class C9Exercise1 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 9.1:\nRandom string generation";
            }

            private Font defaultFont;
            private readonly string targetString = "cat";
            private string currentString;
            private int iterations;
            private int foundAtIteration = -1;

            public override void _Ready()
            {
                defaultFont = Assets.SimpleDefaultFont.Regular;
            }

            public override void _Process(float delta)
            {
                currentString = StringGenerator.Generate(targetString.Length);

                if (foundAtIteration == -1 && currentString == targetString)
                {
                    foundAtIteration = iterations;
                }

                iterations++;
                Update();
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawString(defaultFont, new Vector2(20, size.y - 150), "Target string: " + targetString);
                DrawString(defaultFont, new Vector2(20, size.y - 125), "Random string: " + currentString);
                DrawString(defaultFont, new Vector2(20, size.y - 100), "Current iterations: " + iterations);
                DrawString(defaultFont, new Vector2(20, size.y - 75), "Target found at: " + ((foundAtIteration >= 0) ? foundAtIteration.ToString() : "N/A"));
            }
        }

        public static class StringGenerator
        {
            private const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

            public static string Generate(int length)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < length; ++i)
                {
                    sb.Append(ALPHABET[MathUtils.RandRangei(0, ALPHABET.Length - 1)]);
                }
                return sb.ToString();
            }
        }
    }
}
