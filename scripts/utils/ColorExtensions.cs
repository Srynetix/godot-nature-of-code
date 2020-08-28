using Godot;

public static class ColorExtensions
{
  public static Color WithAlpha(this Color color, byte alpha)
  {
    Color clone = color;
    clone.a8 = alpha;
    return clone;
  }
}
