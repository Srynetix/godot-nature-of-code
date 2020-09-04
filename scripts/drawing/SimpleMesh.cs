using Godot;

namespace Drawing
{
  /// <summary>
  /// Simple "mesh" drawing. Can be a circle, square, custom drawing or custom texture.
  /// </summary>
  public class SimpleMesh : Node2D
  {
    /// <summary>
    /// Mesh type enum value.
    /// </summary>
    public enum TypeEnum
    {
      /// <summary>Circle mesh</summary>
      Circle,
      /// <summary>Square mesh</summary>
      Square,
      /// <summary>Custom mesh</summary>
      Custom,
      /// <summary>Custom texture</summary>
      Texture
    }

    /// <summary>Custom draw function definition</summary>
    public delegate void CustomDrawFunc(SimpleMesh pen);

    /// <summary>Mesh size</summary>
    public Vector2 MeshSize = new Vector2(40, 40);
    /// <summary>Mesh type</summary>
    public TypeEnum MeshType = TypeEnum.Circle;
    /// <summary>Custom draw method</summary>
    public CustomDrawFunc CustomDrawMethod = null;
    /// <summary>Custom texture</summary>
    public Texture CustomTexture = null;
    /// <summary>Custom material</summary>
    public Material CustomMaterial = null;

    private Sprite sprite;
    private SimpleCircleSprite circleSprite;

    /// <summary>
    /// Create a default light-cyan circle mesh.
    /// </summary>
    public SimpleMesh()
    {
      Modulate = Colors.LightCyan;
      Name = "SimpleMesh";
    }

    public override void _Ready()
    {
      sprite = new Sprite();
      sprite.Name = "CustomSprite";
      sprite.Material = CustomMaterial;
      sprite.Modulate = Modulate;
      sprite.Visible = false;
      AddChild(sprite);

      circleSprite = new SimpleCircleSprite();
      circleSprite.Name = "CircleSprite";
      circleSprite.Modulate = Modulate;
      circleSprite.Visible = false;
      AddChild(circleSprite);

      if (MeshType == TypeEnum.Texture)
      {
        if (CustomTexture != null)
        {
          sprite.Texture = CustomTexture;
          sprite.Scale = MeshSize / sprite.Texture.GetSize();
          sprite.Visible = Visible;
        }
      }

      else if (MeshType == TypeEnum.Circle)
      {
        circleSprite.Radius = MeshSize.x / 2;
        circleSprite.Visible = Visible;
      }
    }

    public override void _Draw()
    {
      if (MeshType == TypeEnum.Square)
      {
        DrawRect(new Rect2(-MeshSize / 2, MeshSize / 2), Modulate);
      }

      else if (MeshType == TypeEnum.Custom)
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
}
