using Godot;

public enum SimpleMeshTypeEnum
{
  Round,
  Square,
  Custom,
  Texture
}

public enum SimpleDefaultTextureEnum
{
  WhiteDot,
  WhiteDotAlpha
}

public static class SimpleDefaultTexture
{
  public static Texture WhiteDotTexture;
  public static Texture WhiteDotAlphaTexture;

  public static void Initialize()
  {
    if (WhiteDotTexture == null)
    {
      WhiteDotTexture = (Texture)GD.Load("res://assets/textures/white-dot-on-black.png");
    }

    if (WhiteDotAlphaTexture == null)
    {
      WhiteDotAlphaTexture = (Texture)GD.Load("res://assets/textures/white-dot-blur.png");
    }
  }

  public static Texture FromEnum(SimpleDefaultTextureEnum value)
  {
    Initialize();

    if (value == SimpleDefaultTextureEnum.WhiteDot)
    {
      return WhiteDotTexture;
    }

    else
    {
      return WhiteDotAlphaTexture;
    }
  }
}

public class SimpleMesh : Node2D
{
  public delegate void CustomDraw(SimpleMesh pen);

  public Color OutlineColor = Colors.LightBlue;
  public float OutlineWidth = 2f;
  public Vector2 BodySize = new Vector2(40, 40);
  public SimpleMeshTypeEnum MeshType = SimpleMeshTypeEnum.Round;
  public CustomDraw CustomDrawMethod = null;
  public Texture CustomTexture = null;
  public CanvasItemMaterial.BlendModeEnum CustomTextureBlendMode = CanvasItemMaterial.BlendModeEnum.Mix;
  public bool Drawing
  {
    get => _drawing;
    set
    {
      _drawing = value;
      if (sprite != null)
      {
        sprite.Visible = value && MeshType == SimpleMeshTypeEnum.Texture;
      }
    }
  }

  public Color BaseColor
  {
    get => _baseColor;
    set
    {
      _baseColor = value;
      if (sprite != null)
      {
        sprite.Modulate = value;
      }
    }
  }

  private CanvasItemMaterial material;
  private Sprite sprite;

  private Color _baseColor = Colors.White;
  private bool _drawing = true;

  public override void _Ready()
  {
    material = new CanvasItemMaterial();
    material.BlendMode = CustomTextureBlendMode;

    sprite = new Sprite();
    sprite.Material = material;
    sprite.Modulate = BaseColor;
    sprite.Visible = false;
    AddChild(sprite);

    if (MeshType == SimpleMeshTypeEnum.Texture)
    {
      if (CustomTexture != null)
      {
        sprite.Texture = CustomTexture;
        sprite.Scale = BodySize / sprite.Texture.GetSize();
        sprite.Visible = _drawing;
      }
    }
  }

  public override void _Draw()
  {
    if (!Drawing)
    {
      return;
    }

    if (MeshType == SimpleMeshTypeEnum.Round)
    {
      DrawCircle(Vector2.Zero, BodySize.x / 2, OutlineColor);
      DrawCircle(Vector2.Zero, BodySize.x / 2 - OutlineWidth, BaseColor);
    }

    else if (MeshType == SimpleMeshTypeEnum.Square)
    {
      var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
      DrawRect(new Rect2(-BodySize / 2, BodySize / 2), OutlineColor);
      DrawRect(new Rect2(-BodySize / 2 + outlineVec / 2, BodySize / 2 - outlineVec / 2), OutlineColor);
    }

    else if (MeshType == SimpleMeshTypeEnum.Custom)
    {
      if (CustomDrawMethod != null)
      {
        CustomDrawMethod(this);
      }
    }
  }

  public override void _Process(float delta)
  {
    Update();
  }
}
