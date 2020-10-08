using Godot;

/// <summary>
/// Color extension methods
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Clone a color with an alpha value.
    /// </summary>
    /// <param name="color">Current color</param>
    /// <param name="alpha">Alpha value (0-255)</param>
    /// <returns>Color with alpha</returns>
    public static Color WithAlpha(this Color color, byte alpha)
    {
        Color clone = color;
        clone.a8 = alpha;
        return clone;
    }
}
