using Godot;

/// <summary>
/// Vector extension methods
/// </summary>
public static class VectorExtensions
{
  /// <summary>
  /// Normalize a vector using a specific length.
  /// </summary>
  /// <param name="vector">Current vector</param>
  /// <param name="length">Target length</param>
  /// <returns>Normalized vector</returns>
  public static Vector2 NormalizeTo(this Vector2 vector, float length)
  {
    var vec = new Vector2(vector.x, vector.y);
    var len = vec.Length();
    if (len > 0)
    {
      len /= length;
      vec.x *= len;
      vec.y *= len;
    }
    return vec;
  }

  /// <summary>
  /// Add a random value to X and Y components.
  /// </summary>
  /// <param name="vector">Current vector</param>
  /// <param name="coef">Coefficient</param>
  /// <returns>Vector with jitter</returns>
  public static Vector2 Jitter(this Vector2 vector, float coef)
  {
    var vec = new Vector2(vector.x, vector.y);
    vec.x += MathUtils.Randf() * coef;
    vec.y += MathUtils.Randf() * coef;
    return vec;
  }

  /// <summary>
  /// Get a normal point on AB from P.
  /// </summary>
  /// <param name="p">Current vector</param>
  /// <param name="a">Start point</param>
  /// <param name="b">End point</param>
  /// <returns>Normal point</returns>
  public static Vector2 GetNormalPoint(this Vector2 p, Vector2 a, Vector2 b)
  {
    var ap = p - a;
    var ab = (b - a).Normalized();
    var dot = ap.Dot(ab);
    return a + (ab * dot);
  }
}
