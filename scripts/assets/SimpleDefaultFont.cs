using Godot;

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
}
