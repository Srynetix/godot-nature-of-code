using Godot;

/// <summary>
/// Math utility functions
/// </summary>
public static class MathUtils
{
  /// <summary>
  /// Map a value from one bound to another.
  /// </summary>
  /// <param name="value">Value to map</param>
  /// <param name="istart">Original lower bound</param>
  /// <param name="istop">Original upper bound</param>
  /// <param name="ostart">Target lower bound</param>
  /// <param name="ostop">Target upper bound</param>
  /// <returns>Mapped value</returns>
  static public float Map(float value, float istart, float istop, float ostart, float ostop)
  {
    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
  }

  /// <summary>
  /// Return a signed Randf, between -1 and 1.
  /// </summary>
  /// <returns>Signed random float</returns>
  static public float SignedRandf()
  {
    return Map((float)GD.RandRange(0, 1), 0, 1, -1, 1);
  }

  /// <summary>
  /// Return a float RandRange.
  /// </summary>
  /// <param name="min">Lower bound</param>
  /// <param name="max">Upper bound</param>
  /// <returns>Random float</returns>
  static public float RandRangef(float min, float max)
  {
    return (float)GD.RandRange(min, max);
  }

  /// <summary>
  /// Return a Vector with X in a random range.
  /// </summary>
  /// <param name="minX">X lower bound</param>
  /// <param name="maxX">X upper bound</param>
  /// <param name="y">Fixed Y value</param>
  /// <returns>Random vector</returns>
  static public Vector2 RandRangeVector2X(float minX, float maxX, float y)
  {
    return new Vector2(RandRangef(minX, maxX), y);
  }

  /// <summary>
  /// Return a Vector with Y in a random range.
  /// </summary>
  /// <param name="x">Fixed X value</param>
  /// <param name="minY">Y lower bound</param>
  /// <param name="maxY">Y upper bound</param>
  /// <returns>Random vector</returns>
  static public Vector2 RandRangeVector2Y(float x, float minY, float maxY)
  {
    return new Vector2(x, RandRangef(minY, maxY));
  }

  /// <summary>
  /// Return a Vector with coordinates in a random range (vector limits).
  /// </summary>
  /// <param name="rangeX">X bounds</param>
  /// <param name="rangeY">Y bounds</param>
  /// <returns>Random vector</returns>
  static public Vector2 RandVector2(Vector2 rangeX, Vector2 rangeY)
  {
    return RandVector2(rangeX.x, rangeX.y, rangeY.x, rangeY.y);
  }

  /// <summary>
  /// Return a Vector with coordinates in a random range (float limits).
  /// </summary>
  /// <param name="minX">X lower bound</param>
  /// <param name="maxX">X upper bound</param>
  /// <param name="minY">Y lower bound</param>
  /// <param name="maxY">Y upper bound</param>
  /// <returns>Random vector</returns>
  static public Vector2 RandVector2(float minX, float maxX, float minY, float maxY)
  {
    var vec = Vector2.Zero;
    vec.x = (float)GD.RandRange(minX, maxX);
    vec.y = (float)GD.RandRange(minY, maxY);

    return vec;
  }

  /// <summary>
  /// Return a random color.
  /// </summary>
  /// <returns>Random color</returns>
  static public Color RandColor()
  {
    return new Color((float)GD.RandRange(0, 1), (float)GD.RandRange(0, 1), (float)GD.RandRange(0, 1));
  }
}
