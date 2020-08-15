using Godot;

public class DrawCanvas : Control
{
  public delegate void DrawFunction(Node2D pen);
  public TextureRect board;

  private const int WILL_CLEAR_FRAMES_COUNT = 2;

  private Viewport viewport;
  private Node2D pen;
  private DrawFunction drawFunction = null;
  private int willClearFrames = 0;
  private Color clearColor = Colors.Black;

  public DrawCanvas(DrawFunction func = null)
  {
    drawFunction = func;
  }

  public void SetDrawFunction(DrawFunction fn)
  {
    drawFunction = fn;
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

  public void QueueClearDrawing(Color color)
  {
    willClearFrames = WILL_CLEAR_FRAMES_COUNT;
    clearColor = color;
  }

  public void OnPenDraw()
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

    if (drawFunction != null)
    {
      drawFunction(pen);
    }
  }

  public override void _Process(float delta)
  {
    pen.Update();
  }
}
