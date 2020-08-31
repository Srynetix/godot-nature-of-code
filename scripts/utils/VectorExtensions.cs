using Godot;

public static class VectorExtensions
{
  public static Vector2 NormalizeTo(this Vector2 vector, float length)
  {
    var vec = new Vector2(vector.x, vector.y);
    var len = vec.Length();
    if (len > 0)
    {
      len = len / length;
      vec.x *= len;
      vec.y *= len;
    }
    return vec;
  }

  public static Vector2 Jitter(this Vector2 vector, float coef)
  {
    var vec = new Vector2(vector.x, vector.y);
    vec.x += MathUtils.Randf() * coef;
    vec.y += MathUtils.Randf() * coef;
    return vec;
  }
}
