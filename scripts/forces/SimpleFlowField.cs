using Godot;

namespace Forces
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

    /// <summary>
    /// Flow direction.
    /// </summary>
    protected class FlowDirection : Control
    {
      ///<summary>Direction vector</summary>
      public Vector2 Direction = Vector2.Right;

      #region Lifecycle methods

      public override void _Draw()
      {
        float arrowOffset = 8;
        float arrowWidth = 4;
        float arrowEndLength = 8;
        float arrowEndWidth = 8;

        DrawRect(new Rect2(arrowOffset / 2, RectSize.y / 2 - arrowWidth / 2, RectSize.x - arrowOffset, arrowWidth), Colors.Lavender.WithAlpha(32));
        DrawRect(new Rect2(RectSize.x - arrowEndLength, RectSize.y / 2 - arrowEndWidth / 2, arrowEndLength / 2, arrowEndWidth), Colors.Lavender.WithAlpha(64));
      }

      public override void _Process(float delta)
      {
        Update();
      }

      #endregion
    }

    /// <summary>
    /// Lookup direction from flow.
    /// </summary>
    /// <param name="position">Position</param>
    /// <returns>Direction vector</returns>
    public Vector2 Lookup(Vector2 position)
    {
      int x = (int)Mathf.Clamp(position.x / Resolution, 0, cols - 1);
      int y = (int)Mathf.Clamp(position.y / Resolution, 0, rows - 1);
      return field[x + y * cols].Direction;
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

    #region Lifecycle methods

    public override void _Ready()
    {
      CreateFieldFromWindowSize(GetViewportRect().Size);
    }

    public override void _Draw()
    {
      // Draw grid
      float bgOffset = 2;

      for (int j = 0; j < rows; ++j)
      {
        for (int i = 0; i < cols; ++i)
        {
          DrawRect(new Rect2(i * Resolution + bgOffset / 2, j * Resolution + bgOffset / 2, Resolution - bgOffset, Resolution - bgOffset), Colors.Beige.WithAlpha(16));
        }
      }
    }

    #endregion

    #region Private methods

    private void CreateFieldFromWindowSize(Vector2 screenSize)
    {
      var resolutionSize = new Vector2(Resolution, Resolution);
      cols = (int)(screenSize.x / Resolution);
      rows = (int)(screenSize.y / Resolution);
      field = new FlowDirection[cols * rows];

      for (int j = 0; j < rows; ++j)
      {
        for (int i = 0; i < cols; ++i)
        {
          var idx = i + j * cols;
          var direction = new FlowDirection();
          direction.RectSize = resolutionSize;
          direction.RectPosition = new Vector2(i * Resolution, j * Resolution);
          direction.RectPivotOffset = resolutionSize / 2.0f;
          direction.Direction = ComputeDirectionFromPosition(i, j);
          direction.RectRotation = Mathf.Rad2Deg(direction.Direction.Angle());
          field[idx] = direction;
          AddChild(direction);
        }
      }
    }

    #endregion
  }
}
