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
  public Color BaseColor = Colors.White;
  public bool OutlineOnly = false;

  private Vector2 lineA = Vector2.Zero;
  private Vector2 lineB = Vector2.Zero;
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
}
