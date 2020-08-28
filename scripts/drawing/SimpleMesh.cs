using Godot;

public enum SimpleMeshTypeEnum
{
  Round,
  Square,
  Custom,
  Texture
}

public class SimpleMesh : Node2D
{
  public delegate void CustomDraw(SimpleMesh pen);

  public Vector2 MeshSize = new Vector2(40, 40);
  public SimpleMeshTypeEnum MeshType = SimpleMeshTypeEnum.Round;
  public CustomDraw CustomDrawMethod = null;
  public Texture CustomTexture = null;
  public CanvasItemMaterial.BlendModeEnum CustomTextureBlendMode = CanvasItemMaterial.BlendModeEnum.Mix;

  private CanvasItemMaterial material;
  private Sprite sprite;
  private SimpleCircleSprite circleSprite;

  public SimpleMesh()
  {
    Modulate = Colors.LightCyan;
  }

  public override void _Ready()
  {
    material = new CanvasItemMaterial();
    material.BlendMode = CustomTextureBlendMode;

    sprite = new Sprite();
    sprite.Material = material;
    sprite.Modulate = Modulate;
    sprite.Visible = false;
    AddChild(sprite);

    circleSprite = new SimpleCircleSprite();
    circleSprite.Modulate = Modulate;
    circleSprite.Visible = false;
    AddChild(circleSprite);

    if (MeshType == SimpleMeshTypeEnum.Texture)
    {
      if (CustomTexture != null)
      {
        sprite.Texture = CustomTexture;
        sprite.Scale = MeshSize / sprite.Texture.GetSize();
        sprite.Visible = Visible;
      }
    }

    else if (MeshType == SimpleMeshTypeEnum.Round)
    {
      circleSprite.Radius = MeshSize.x / 2;
      circleSprite.Visible = Visible;
    }
  }

  public override void _Draw()
  {
    if (MeshType == SimpleMeshTypeEnum.Square)
    {
      DrawRect(new Rect2(-MeshSize / 2, MeshSize / 2), Modulate);
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

  private void UpdateColor()
  {
    if (sprite != null)
    {
      sprite.Modulate = Modulate;
    }

    if (sprite != null)
    {
      circleSprite.Modulate = Modulate;
    }
  }
}
