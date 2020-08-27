using Godot;

public class SimpleLineSprite : Sprite
{
  public Vector2 LineA
  {
    get => lineA;
    set
    {
      lineA = value;
      ApplyRotationAndScale(updatePosition: true);
    }
  }

  public Vector2 LineB
  {
    get => lineB;
    set
    {
      lineB = value;
      ApplyRotationAndScale();
    }
  }

  public float Width
  {
    get => width;
    set
    {
      width = value;
      Scale = new Vector2(width, Scale.y);
    }
  }

  public bool Drawing
  {
    get => drawing;
    set
    {
      drawing = value;
      UpdateDrawing();
    }
  }

  public Color BaseColor
  {
    get => baseColor;
    set
    {
      baseColor = value;
      UpdateDrawing();
    }
  }

  private Vector2 lineA = Vector2.Zero;
  private Vector2 lineB = Vector2.Zero;
  private Color baseColor = Colors.White;
  private bool drawing = true;
  private float width = 1;

  public override void _Ready()
  {
    Texture = SimpleDefaultTexture.FromEnum(SimpleDefaultTextureEnum.Line);
    SelfModulate = BaseColor;

    ApplyRotationAndScale(updatePosition: true);
  }

  private void ApplyRotationAndScale(bool updatePosition = false)
  {
    var dir = lineB - lineA;
    Rotation = dir.Angle() + Mathf.Pi / 2;
    if (Texture != null)
    {
      Scale = new Vector2(Scale.x, dir.Length() / Texture.GetSize().y);
    }

    if (updatePosition)
    {
      GlobalPosition = lineA + dir / 2;
    }
  }

  private void UpdateDrawing()
  {
    if (drawing)
    {
      Modulate = baseColor;
    }
    else
    {
      Modulate = Colors.Transparent;
    }
  }
}
