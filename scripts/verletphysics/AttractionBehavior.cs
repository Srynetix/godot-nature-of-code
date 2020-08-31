namespace VerletPhysics
{
  /// <summary>
  /// Attraction behavior.
  /// </summary>
  public class AttractionBehavior : IBehavior
  {
    /// <summary>Attraction point</summary>
    public VerletPoint Attractor;
    /// <summary>Jitter value (random coefficient)</summary>
    public float Jitter;
    /// <summary>Attraction strength</summary>
    public float Strength;
    /// <summary>Attraction radius</summary>
    public float Radius
    {
      get => radius;
      set
      {
        radius = value;
        radiusSquared = value * value;
      }
    }

    private float radius;
    private float radiusSquared;

    /// <summary>
    /// Create an attraction behavior.
    /// </summary>
    /// <param name="attractor">Attraction point</param>
    /// <param name="strength">Attraction strength</param>
    /// <param name="radius">Attraction radius</param>
    /// <param name="jitter">Random coefficient</param>
    public AttractionBehavior(VerletPoint attractor, float strength, float radius, float jitter = 0)
    {
      Attractor = attractor;
      Strength = strength;
      Radius = radius;
      Jitter = jitter;
    }

    public void ApplyBehavior(VerletPoint point, float delta)
    {
      var diff = Attractor.GlobalPosition - point.GlobalPosition;
      var dist = diff.LengthSquared();
      if (dist < radiusSquared)
      {
        var f = diff.NormalizeTo((1.0f - dist / radiusSquared)).Jitter(Jitter) * Strength * delta;
        point.ApplyForce(f);
      }
    }
  }
}
