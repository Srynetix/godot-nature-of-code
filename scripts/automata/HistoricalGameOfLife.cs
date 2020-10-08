using Godot;

namespace Automata
{
    public class HistoricalCell : BoolCell
    {
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

        public override void _Process(float delta)
        {
            base._Process(delta);

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

    /// <summary>
    /// Historical Game of Life.
    /// </summary>
    public class HistoricalGameOfLife : CellularAutomata2D<HistoricalCell, bool> { }
}
