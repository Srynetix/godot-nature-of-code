using Godot;

public enum SimpleDefaultTextureEnum
{
  WhiteDot,
  WhiteDotAlpha,
  WhiteDotAlphaWithOutline,
  WhiteDotOutlineOnly,
  WhiteDotBlur,
  Line
}

public enum SimpleDefaultMaterialEnum
{
  Add
}

public static class SimpleDefaultMaterial
{
  private static Material AddMaterial;

  public static void Initialize()
  {
    if (AddMaterial == null)
    {
      var material = new CanvasItemMaterial();
      material.BlendMode = CanvasItemMaterial.BlendModeEnum.Add;
      AddMaterial = material;
    }
  }

  public static Material FromEnum(SimpleDefaultMaterialEnum value)
  {
    Initialize();

    if (value == SimpleDefaultMaterialEnum.Add)
    {
      return AddMaterial;
    }

    return null;
  }
}

public static class SimpleDefaultTexture
{
  private static Texture WhiteDotTexture;
  private static Texture WhiteDotAlphaTexture;
  private static Texture WhiteDotAlphaWithOutlineTexture;
  private static Texture WhiteDotOutlineOnlyTexture;
  private static Texture WhiteDotBlurTexture;
  private static Texture LineTexture;

  public static void Initialize()
  {
    if (WhiteDotTexture == null)
    {
      WhiteDotTexture = (Texture)GD.Load("res://assets/textures/white-dot-on-black.png");
    }

    if (WhiteDotBlurTexture == null)
    {
      WhiteDotBlurTexture = (Texture)GD.Load("res://assets/textures/white-dot-blur.png");
    }

    if (WhiteDotAlphaTexture == null)
    {
      WhiteDotAlphaTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha.png");
    }

    if (WhiteDotOutlineOnlyTexture == null)
    {
      WhiteDotOutlineOnlyTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha-outline.png");
    }

    if (WhiteDotAlphaWithOutlineTexture == null)
    {
      WhiteDotAlphaWithOutlineTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha-with-outline.png");
    }

    if (LineTexture == null)
    {
      LineTexture = (Texture)GD.Load("res://assets/textures/line.png");
    }
  }

  public static Texture FromEnum(SimpleDefaultTextureEnum value)
  {
    Initialize();

    if (value == SimpleDefaultTextureEnum.WhiteDot)
    {
      return WhiteDotTexture;
    }

    else if (value == SimpleDefaultTextureEnum.WhiteDotBlur)
    {
      return WhiteDotBlurTexture;
    }

    else if (value == SimpleDefaultTextureEnum.WhiteDotOutlineOnly)
    {
      return WhiteDotOutlineOnlyTexture;
    }

    else if (value == SimpleDefaultTextureEnum.WhiteDotAlphaWithOutline)
    {
      return WhiteDotAlphaWithOutlineTexture;
    }

    else if (value == SimpleDefaultTextureEnum.WhiteDotAlpha)
    {
      return WhiteDotAlphaTexture;
    }

    else if (value == SimpleDefaultTextureEnum.Line)
    {
      return LineTexture;
    }

    return null;
  }
}

public static class SimpleDefaultFont
{
  private static Font Regular = null;

  static public Font LoadDefaultFont()
  {
    if (Regular == null)
    {
      var fontData = GD.Load("res://assets/fonts/Raleway-Regular.ttf");
      var dynamicFont = new DynamicFont();
      dynamicFont.FontData = (DynamicFontData)fontData;
      dynamicFont.Size = 16;
      dynamicFont.UseFilter = true;
      dynamicFont.OutlineSize = 1;
      dynamicFont.OutlineColor = Colors.Black;
      Regular = dynamicFont;
    }

    return Regular;
  }
}
