using Godot;
using Automata;

namespace Examples.Chapter7
{
    /// <summary>
    /// Exercise 7.14: Moving Game of Life.
    /// </summary>
    public class C7Exercise14 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Exercise 7.14:\nMoving Game of Life";
        }

        private class MovingCell : BoolCell
        {
            private Vector2 _basePosition;
            private Vector2 _idx;
            private float _t;
            private float _acc;

            protected override Color? GetStateColor()
            {
                var baseColor = base.GetStateColor();
                if (baseColor.HasValue)
                {
                    return baseColor.Value.Darkened(_acc / 255.0f);
                }
                return baseColor;
            }

            public override void _Ready()
            {
                base._Ready();

                _basePosition = Position;
                _idx = _basePosition / GetViewportRect().Size;
            }

            public override void _Process(float delta)
            {
                base._Process(delta);

                var spe = new Vector2(20, 10);
                var amp = new Vector2(5, 2);

                var pos = Vector2.Zero;
                pos.x += Mathf.Sin(_t * _idx.y * spe.x) * amp.x;
                pos.y += Mathf.Cos(_t * _idx.y * spe.y) * amp.y;
                Position = _basePosition + pos;

                _t += delta;

                if (IsAlive())
                {
                    _acc = Mathf.Clamp(_acc + (delta * 100), 0, 255);
                }
                else
                {
                    _acc = 0;
                }
            }
        }

        private class MovingGameOfLife : CellularAutomata2D<MovingCell, bool> { }

        public override void _Ready()
        {
            var ca = new MovingGameOfLife()
            {
                TouchBehavior = TouchBehaviorEnum.DrawCell,
                WrapBehavior = WrapBehaviorEnum.Wrap,
            };
            AddChild(ca);

            ca.RandomizeGrid();
        }
    }
}
