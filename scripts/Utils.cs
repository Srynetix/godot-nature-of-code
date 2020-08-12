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

public class Utils
{
  /**
   * Map a value from one bound to another.
   */
  static public float Map(float value, float istart, float istop, float ostart, float ostop)
  {
    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
  }

  /**
   * Return a signed Randf, between -1 and 1.
   */
  static public float SignedRandf()
  {
    return Map(GD.Randf(), 0, 1, -1, 1);
  }

  /**
   * Return a random Vector2.
   */
  static public Vector2 RandVector2(Vector2 min, Vector2 max)
  {
    var vec = Vector2.Zero;
    vec.x = (float)GD.RandRange(min.x, max.x);
    vec.y = (float)GD.RandRange(min.y, max.y);

    return vec;
  }

  static public Color RandColor()
  {
    return new Color(GD.Randf(), GD.Randf(), GD.Randf());
  }

  static private Font DEFAULT_FONT = null;

  static public Font LoadDefaultFont()
  {
    if (DEFAULT_FONT == null)
    {
      var fontData = GD.Load("res://assets/fonts/Raleway-Regular.ttf");
      var dynamicFont = new DynamicFont();
      dynamicFont.FontData = (DynamicFontData)fontData;
      dynamicFont.Size = 16;
      dynamicFont.UseFilter = true;
      dynamicFont.OutlineSize = 1;
      dynamicFont.OutlineColor = Colors.Black;
      DEFAULT_FONT = dynamicFont;
    }

    return DEFAULT_FONT;
  }
}
