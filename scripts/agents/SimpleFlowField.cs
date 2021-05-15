using Godot;

namespace Agents
{
    /// <summary>
    /// Simple flow field.
    /// </summary>
    public class SimpleFlowField : Node2D
    {
        /// <summary>Grid resolution</summary>
        public int Resolution = 30;

        /// <summary>Column count</summary>
        protected int cols;

        /// <summary>Row count</summary>
        protected int rows;

        /// <summary>Flow field</summary>
        protected FlowDirection[] field;

        /// <summary>Size</summary>
        protected Vector2 size;

        /// <summary>
        /// Flow direction.
        /// </summary>
        protected class FlowDirection : Control
        {
            ///<summary>Direction vector</summary>
            public Vector2 Direction = Vector2.Right;

            public override void _Draw()
            {
                const float arrowOffset = 8;
                const float arrowWidth = 4;
                const float arrowEndLength = 8;
                const float arrowEndWidth = 4;

                DrawRect(new Rect2(arrowOffset / 2, (RectSize.y / 2) - (arrowWidth / 2), RectSize.x - arrowOffset, arrowWidth), Colors.Lavender.WithAlpha(32));
                DrawRect(new Rect2(RectSize.x - arrowEndLength, (RectSize.y / 2) - (arrowEndWidth / 2), arrowEndLength / 2, arrowEndWidth), Colors.White.WithAlpha(128));
            }

            public override void _Process(float delta)
            {
                Update();
            }
        }

        /// <summary>
        /// Lookup direction from flow.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Direction vector</returns>
        public Vector2 Lookup(Vector2 position)
        {
            var rect = new Rect2(GlobalPosition, size);
            if (rect.HasPoint(position))
            {
                var x = (int)Mathf.Clamp((position.x - GlobalPosition.x) / Resolution, 0, cols - 1);
                var y = (int)Mathf.Clamp((position.y - GlobalPosition.y) / Resolution, 0, rows - 1);
                return field[x + (y * cols)].Direction;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Compute flow direction from a given position.
        /// Defaults with a right vector.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Flow direction</returns>
        protected virtual Vector2 ComputeDirectionFromPosition(int x, int y)
        {
            return Vector2.Right;
        }

        /// <summary>
        /// Refresh flow directions.
        /// </summary>
        protected void RefreshDirections()
        {
            for (int j = 0; j < rows; ++j)
            {
                for (int i = 0; i < cols; ++i)
                {
                    var idx = i + (j * cols);
                    var direction = field[idx];
                    direction.Direction = ComputeDirectionFromPosition(i, j);
                    direction.RectRotation = Mathf.Rad2Deg(direction.Direction.Angle());
                }
            }
        }

        public override void _Ready()
        {
            CreateFieldFromWindowSize(GetViewportRect().Size);
        }

        public override void _Draw()
        {
            // Draw grid
            const float bgOffset = 2;

            for (int j = 0; j < rows; ++j)
            {
                for (int i = 0; i < cols; ++i)
                {
                    DrawRect(new Rect2((i * Resolution) + (bgOffset / 2), (j * Resolution) + (bgOffset / 2), Resolution - bgOffset, Resolution - bgOffset), Colors.Beige.WithAlpha(16));
                }
            }
        }

        private void CreateFieldFromWindowSize(Vector2 screenSize)
        {
            var resolutionSize = new Vector2(Resolution, Resolution);
            size = screenSize;
            cols = (int)(screenSize.x / Resolution);
            rows = (int)(screenSize.y / Resolution);
            field = new FlowDirection[cols * rows];

            for (int j = 0; j < rows; ++j)
            {
                for (int i = 0; i < cols; ++i)
                {
                    var idx = i + (j * cols);
                    var direction = new FlowDirection()
                    {
                        RectSize = resolutionSize,
                        RectPosition = new Vector2(i * Resolution, j * Resolution),
                        RectPivotOffset = resolutionSize / 2.0f,
                        Direction = ComputeDirectionFromPosition(i, j)
                    };
                    direction.RectRotation = Mathf.Rad2Deg(direction.Direction.Angle());
                    field[idx] = direction;
                    AddChild(direction);
                }
            }
        }
    }
}
