namespace VerletPhysics
{
  public class AttractionBehavior : IBehavior
  {
    public VerletPoint Attractor;
    public float Jitter;
    public float Strength;

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
