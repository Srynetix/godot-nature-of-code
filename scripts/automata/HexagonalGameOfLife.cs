using Godot;

namespace Automata
{
    public class HexagonalCell : BoolCell
    {
        private Sprite sprite;

        public override void _Ready()
        {
            base._Ready();

            sprite = new Sprite()
            {
                Texture = Assets.SimpleDefaultTexture.HexagonTexture
            };
            sprite.Scale = Size / sprite.Texture.GetSize() * 1.25f;
            sprite.Offset = sprite.Texture.GetSize() / 2;
            AddChild(sprite);
        }

        public override void _Process(float delta)
        {
            var color = GetStateColor();
            sprite.Modulate = color ?? Colors.Black;
        }

        // Draw not needed
        public override void _Draw() { }
    }

    public class HexagonalGameOfLife : CellularAutomata2D<HexagonalCell, bool>
    {
        public override void RandomizeGrid()
        {
            _generation = 0;
            int offset = (WrapBehavior == WrapBehaviorEnum.Wrap) ? 0 : 1;

            for (int j = offset; j < _rows - offset; ++j)
            {
                for (int i = offset; i < _cols - offset; ++i)
                {
                    if (i % 2 != j % 2)
                    {
                        continue;
                    }

                    var currPos = i + (j * _cols);
                    var cell = _grid[currPos];
                    cell.State = cell.PreviousState = MathUtils.RandRangei(0, 1) == 1;
                }
            }
        }

        protected override void InitializeGrid()
        {
            var size = GetViewportRect().Size;
            _scale = 20;

            // Ensure _cols and _rows are even
            _cols = Mathf.FloorToInt(size.x / _scale);
            _rows = Mathf.FloorToInt(size.y / _scale * 1.5f);
            _cols = (_cols % 2 != 0) ? _cols - 1 : _cols;
            _rows = (_rows % 2 != 0) ? _rows - 1 : _rows;
            _grid = new HexagonalCell[_cols * _rows];

            for (int j = 0; j < _rows; ++j)
            {
                for (int i = 0; i < _cols; ++i)
                {
                    // Ignore some cells (double-coordinates, double-height)
                    if (i % 2 != j % 2)
                    {
                        continue;
                    }

                    var currPos = i + (j * _cols);
                    var xPos = i * _scale;
                    var yPos = j * _scale / 1.5f;
                    var cell = new HexagonalCell()
                    {
                        Position = new Vector2(xPos, yPos),
                        Size = new Vector2(_scale, _scale),
                        PreviousState = false,
                        State = false,
                        CellColor = CellColor,
                        HighlightTransitions = HighlightTransitions
                    };
                    InitializeCell(cell, i, j);
                    _gridContainer.AddChild(cell);
                    _grid[currPos] = cell;
                }
            }
        }

        protected override void ReviveCellAtScreenPos(Vector2 pos)
        {
            // Split position depending on scale
            var xPos = (int)(pos.x / _scale);
            var yPos = (int)(pos.y / _scale * 1.5f);
            int offset = (WrapBehavior == WrapBehaviorEnum.Wrap) ? 0 : 1;
            int x = Mathf.Min(Mathf.Max(offset, xPos), _cols - 1 - offset);
            int y = Mathf.Min(Mathf.Max(offset, yPos), _rows - 1 - offset);

            if (x % 2 != y % 2)
            {
                return;
            }

            var cell = _grid[x + (y * _cols)];
            cell.PreviousState = false;
            cell.State = true;
        }

        protected override int GetAliveNeighborsFromCell(int x, int y)
        {
            int xPos;
            int yPos;
            int count = 0;

            // Top-left
            xPos = WrapX(x, -1);
            yPos = WrapY(y, -1);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;
            // Top
            xPos = x;
            yPos = WrapY(y, -2);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;
            // Top-right
            xPos = WrapX(x, 1);
            yPos = WrapY(y, -1);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;
            // Bottom-right
            xPos = WrapX(x, 1);
            yPos = WrapY(y, 1);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;
            // Bottom
            xPos = x;
            yPos = WrapY(y, 2);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;
            // Bottom-left
            xPos = WrapX(x, -1);
            yPos = WrapY(y, 1);
            count += (_grid[xPos + (yPos * _cols)].PreviousState) ? 1 : 0;

            return count;
        }

        protected override void Generate()
        {
            int offset = (WrapBehavior == WrapBehaviorEnum.Wrap) ? 0 : 1;

            for (int j = offset; j < _rows - offset; ++j)
            {
                for (int i = offset; i < _cols - offset; ++i)
                {
                    if (i % 2 != j % 2)
                    {
                        continue;
                    }

                    _grid[i + (j * _cols)].PreviousState = _grid[i + (j * _cols)].State;
                }
            }

            for (int j = offset; j < _rows - offset; ++j)
            {
                for (int i = offset; i < _cols - offset; ++i)
                {
                    if (i % 2 != j % 2)
                    {
                        continue;
                    }

                    _grid[i + (j * _cols)].State = ApplyRules(i, j);
                }
            }
        }

        private int WrapX(int x, int offset)
        {
            return (WrapBehavior == WrapBehaviorEnum.Wrap)
                ? Mathf.PosMod(x + offset, _cols)
                : x + offset;
        }

        private int WrapY(int y, int offset)
        {
            return (WrapBehavior == WrapBehaviorEnum.Wrap)
                ? Mathf.PosMod(y + offset, _rows)
                : y + offset;
        }
    }
}
