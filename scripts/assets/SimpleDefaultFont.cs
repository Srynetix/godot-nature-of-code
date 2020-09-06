using Godot;

/// <summary>
/// Lazy-loaded assets.
/// </summary>
namespace Assets
{
  /// <summary>
  /// Contains lazy-loaded fonts to use.
  /// </summary>
  public static class SimpleDefaultFont
  {
    private static Font Regular = null;

    /// <summary>
    /// Get or create default font.
    /// </summary>
    /// <returns>Default font</returns>
    static public Font LoadDefaultFont()
    {
      if (Regular == null)
      {
        var fontData = (DynamicFontData)GD.Load("res://assets/fonts/Raleway-Regular.ttf");
        Regular = new DynamicFont
        {
          FontData = fontData,
          Size = 16,
          UseFilter = true,
          OutlineSize = 1,
          OutlineColor = Colors.Black
        };
      }

      return Regular;
    }
  }
}
