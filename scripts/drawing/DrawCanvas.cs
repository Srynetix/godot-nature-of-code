using Godot;

/// <summary>
/// Draw primitives.
/// </summary>
namespace Drawing
{
  /// <summary>
  /// Canvas used to draw shapes without viewport auto-clear.
  /// </summary>
  public class DrawCanvas : Control
  {
    /// <summary>Draw function definition</summary>
    public delegate void DrawFunc(Node2D pen);

    /// <summary>Draw function</summary>
    public DrawFunc DrawFunction = null;

    private const int willClearFramesCount = 2;
    private TextureRect board;
    private Viewport viewport;
    private Node2D pen;
    private int willClearFrames = 0;
    private Color clearColor = Colors.Black;

    /// <summary>
    /// Create a default draw canvas with an optional draw function.
    /// </summary>
    /// <param name="func">Draw function</param>
    public DrawCanvas(DrawFunc func = null)
    {
      DrawFunction = func;
    }

    /// <summary>
    /// Queue a clear draw action with a specific color.
    /// </summary>
    /// <param name="color">Clear color</param>
    public void QueueClearDrawing(Color color)
    {
      willClearFrames = willClearFramesCount;
      clearColor = color;
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      viewport = new Viewport();
      viewport.Size = size;
      viewport.Usage = Viewport.UsageEnum.Usage2d;
      viewport.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
      viewport.RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
      viewport.RenderTargetVFlip = true;
      AddChild(viewport);

      pen = new Node2D();
      pen.Connect("draw", this, nameof(OnPenDraw));
      viewport.AddChild(pen);

      var texture = viewport.GetTexture();
      board = new TextureRect();
      board.AnchorBottom = 1.0f;
      board.AnchorRight = 1.0f;
      board.Texture = texture;
      AddChild(board);

      // First clear on ready
      QueueClearDrawing(clearColor);
    }

    public override void _Process(float delta)
    {
      pen.Update();
    }

    private void OnPenDraw()
    {
      // Clear if needed
      if (willClearFrames > 0)
      {
        willClearFrames--;
        if (willClearFrames == 0)
        {
          pen.DrawRect(GetViewportRect(), clearColor);
        }
      }

      if (DrawFunction != null)
      {
        DrawFunction(pen);
      }
    }
  }
}
