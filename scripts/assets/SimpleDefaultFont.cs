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
    /// <summary>
    /// Regular font.
    /// </summary>
    public static Font Regular
    {
      get => LoadDefaultFont();
    }

    private static Font _regular;

    /// <summary>
    /// Get or create default font.
    /// </summary>
    /// <returns>Default font</returns>
    private static Font LoadDefaultFont()
    {
      if (_regular == null)
      {
        var fontData = (DynamicFontData)GD.Load("res://assets/fonts/Raleway-Regular.ttf");
        _regular = new DynamicFont
        {
          FontData = fontData,
          Size = 16,
          UseFilter = true,
          OutlineSize = 1,
          OutlineColor = Colors.Black
        };
      }

      return _regular;
    }
  }
}
