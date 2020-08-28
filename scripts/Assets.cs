using Godot;

/// <summary>
/// Contains lazy-loaded materials to use.
/// </summary>
public static class SimpleDefaultMaterial
{
  /// <summary>
  /// Material types.
  /// </summary>
  public enum Enum
  {
    /// <summary>Default material with BlendMode.Add</summary>
    Add
  }

  private static Material AddMaterial;

  /// <summary>
  /// Get or create a default material from an enum value.
  /// </summary>
  public static Material FromEnum(Enum value)
  {
    Initialize();

    if (value == Enum.Add)
    {
      return AddMaterial;
    }

    return null;
  }

  private static void Initialize()
  {
    if (AddMaterial == null)
    {
      var material = new CanvasItemMaterial();
      material.BlendMode = CanvasItemMaterial.BlendModeEnum.Add;
      AddMaterial = material;
    }
  }
}

/// <summary>
/// Contains lazy-loaded textures to use.
/// </summary>
public static class SimpleDefaultTexture
{
  /// <summary>
  /// Texture types.
  /// </summary>
  public enum Enum
  {
    /// <summary>Default white circle texture with black background.</summary>
    WhiteDot,
    /// <summary>Default white circle texture with alpha background.</summary>
    WhiteDotAlpha,
    /// <summary>Default white circle texture with outline and alpha background.</summary>
    WhiteDotAlphaWithOutline,
    /// <summary>Default white circle outline with alpha background.</summary>
    WhiteDotOutlineOnly,
    /// <summary>Default white blurry circle with alpha background.</summary>
    WhiteDotBlur,
    /// <summary>Default vertical line texture.</summary>
    Line
  }

  private static Texture WhiteDotTexture;
  private static Texture WhiteDotAlphaTexture;
  private static Texture WhiteDotAlphaWithOutlineTexture;
  private static Texture WhiteDotOutlineOnlyTexture;
  private static Texture WhiteDotBlurTexture;
  private static Texture LineTexture;

  /// <summary>
  /// Get or create a default texture from an enum value.
  /// </summary>
  public static Texture FromEnum(Enum value)
  {
    Initialize();

    if (value == Enum.WhiteDot)
    {
      return WhiteDotTexture;
    }

    else if (value == Enum.WhiteDotBlur)
    {
      return WhiteDotBlurTexture;
    }

    else if (value == Enum.WhiteDotOutlineOnly)
    {
      return WhiteDotOutlineOnlyTexture;
    }

    else if (value == Enum.WhiteDotAlphaWithOutline)
    {
      return WhiteDotAlphaWithOutlineTexture;
    }

    else if (value == Enum.WhiteDotAlpha)
    {
      return WhiteDotAlphaTexture;
    }

    else if (value == Enum.Line)
    {
      return LineTexture;
    }

    return null;
  }

  private static void Initialize()
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
}

/// <summary>
/// Contains lazy-loaded fonts to use.
/// </summary>
public static class SimpleDefaultFont
{
  private static Font Regular = null;

  /// <summary>
  /// Get or create default font.
  /// </summary>
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
