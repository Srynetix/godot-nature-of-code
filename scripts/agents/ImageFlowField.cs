using Godot;

namespace Agents
{
  /// <summary>
  /// Flow field based on a texture.
  /// </summary>
  public class ImageFlowField : SimpleFlowField
  {
    /// <summary>Source texture</summary>
    public Texture SourceTexture;

    /// <summary>Center on screen</summary>
    public bool CenterOnScreen;

    /// <summary>
    /// Create field from source texture.
    /// </summary>
    protected void CreateFieldFromTexture()
    {
      var texSize = SourceTexture.GetSize();
      var resolutionSize = new Vector2(Resolution, Resolution);
      size = texSize;
      cols = (int)(texSize.x / Resolution);
      rows = (int)(texSize.y / Resolution);
      field = new FlowDirection[cols * rows];

      for (int j = 0; j < rows; ++j)
      {
        for (int i = 0; i < cols; ++i)
        {
          var idx = i + (j * cols);
          var direction = new FlowDirection {
            RectSize = resolutionSize,
            RectPosition = new Vector2(i * Resolution, j * Resolution),
            RectPivotOffset = resolutionSize / 2.0f,
            Direction = ComputeDirectionFromPosition(i, j),
          };
          direction.RectRotation = Mathf.Rad2Deg(direction.Direction.Angle());
          field[idx] = direction;
          AddChild(direction);
        }
      }
    }

    /// <summary>
    /// Convert color to direction.
    /// </summary>
    /// <param name="color">Color</param>
    /// <returns>Direction</returns>
    protected Vector2 ColorToDirection(Color color)
    {
      // Use red component as a reference.
      var angle = MathUtils.Map(color.r, 0, 1, 0, Mathf.Pi / 2);
      return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    protected override Vector2 ComputeDirectionFromPosition(int x, int y)
    {
      var image = SourceTexture.GetData();
      image.Lock();
      // Get middle pixel
      var color = image.GetPixel((x * Resolution) + (Resolution / 2), (y * Resolution) + (Resolution / 2));
      image.Unlock();
      return ColorToDirection(color);
    }

    public override void _Ready()
    {
      if (SourceTexture != null)
      {
        CreateFieldFromTexture();

        // Set at the middle of the screen
        if (CenterOnScreen)
        {
          var size = GetViewportRect().Size;
          GlobalPosition = (size / 2) - (SourceTexture.GetSize() / 2);
        }
      }
    }

    public override void _Draw()
    {
      if (SourceTexture != null)
      {
        // Show original texture behind
        DrawTexture(SourceTexture, Vector2.Zero, Colors.White.WithAlpha(128));

        // Then draw grid
        base._Draw();
      }
    }
  }
}
