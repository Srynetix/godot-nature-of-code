using Godot;

namespace Assets
{
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
      /// <summary>Right arrow with alpha background</summary>
      RightArrow,
      /// <summary>Default vertical line texture.</summary>
      Line
    }

    private static Texture WhiteDotTexture;
    private static Texture WhiteDotAlphaTexture;
    private static Texture WhiteDotAlphaWithOutlineTexture;
    private static Texture WhiteDotOutlineOnlyTexture;
    private static Texture WhiteDotBlurTexture;
    private static Texture RightArrowTexture;
    private static Texture LineTexture;

    /// <summary>
    /// Get or create a default texture from an enum value.
    /// </summary>
    /// <param name="value">Texture enum value</param>
    /// <returns>Generated texture</returns>
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

      else if (value == Enum.RightArrow)
      {
        return RightArrowTexture;
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

      if (RightArrowTexture == null)
      {
        RightArrowTexture = (Texture)GD.Load("res://assets/textures/arrow-right.png");
      }

      if (LineTexture == null)
      {
        LineTexture = (Texture)GD.Load("res://assets/textures/line.png");
      }
    }
  }
}
