using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.15: Nested Game of Life.
    /// </summary>
    public class C7Exercise15 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.15:\nNested Game of Life";
        }

        private class SmallGoL : GameOfLife
        {
            public SmallGoL() : base(5) { }

            protected override Vector2 GetAutomataBounds()
            {
                return new Vector2(20, 20);
            }
        }

        private class NestedCell : BoolCell
        {
            private GameOfLife _gameOfLife;

            public override void _Ready()
            {
                base._Ready();

                _gameOfLife = new SmallGoL()
                {
                    HideGUI = true,
                    TouchBehavior = TouchBehaviorEnum.None
                };
                AddChild(_gameOfLife);
                _gameOfLife.RandomizeGrid();
            }

            public override void _Draw()
            {
                var alphaValue = (byte)((IsAlive()) ? 255 : 64);
                _gameOfLife.Modulate = _gameOfLife.Modulate.WithAlpha(alphaValue);
                base._Draw();
            }
        }

        private class NestedGameOfLife : CellularAutomata2D<NestedCell, bool>
        {
            public NestedGameOfLife() : base(20)
            {
                CenterAlignedGrid = true;
                CellColor = Colors.Red.WithAlpha(32);
            }

            protected override Vector2 GetAutomataBounds()
            {
                return new Vector2(400, 400);
            }
        }

        public override void _Ready()
        {
            var ca = new NestedGameOfLife()
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
            };
            AddChild(ca);
            ca.RandomizeGrid();
        }
    }
}
