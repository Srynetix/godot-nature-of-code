using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.12: Image Processing Game of Life.
    /// </summary>
    public class C7Exercise12 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.12:\nImage Processing Game of Life";
        }

        public override void _Ready()
        {
            var ca = new ImageProcessingGameOfLife
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
                SourceTexture = Assets.SimpleDefaultTexture.CloudTexture,
                TextureScale = 8,
            };
            AddChild(ca);
        }
    }
}
