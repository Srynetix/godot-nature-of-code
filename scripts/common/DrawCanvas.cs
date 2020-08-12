using Godot;

public class DrawCanvas : Control
{
  public delegate void DrawFunction(Node2D pen);

  public TextureRect board;

  private Viewport viewport;
  private Node2D pen;
  private Color backgroundColor = Color.Color8(45, 45, 45);
  private DrawFunction drawFunction = null;

  public DrawCanvas(DrawFunction func = null)
  {
    drawFunction = func;
  }

  public void SetDrawFunction(DrawFunction fn)
  {
    drawFunction = fn;
  }

  public void SetBackgroundColor(Color color)
  {
    backgroundColor = color;
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

    pen = new Node2D();
    pen.Connect("draw", this, nameof(OnPenDraw));
    viewport.AddChild(pen);
    AddChild(viewport);

    var texture = viewport.GetTexture();
    board = new TextureRect();
    board.AnchorBottom = 1.0f;
    board.AnchorRight = 1.0f;
    board.Texture = texture;
    AddChild(board);
  }

  public void OnPenDraw()
  {
    VisualServer.SetDefaultClearColor(backgroundColor);

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