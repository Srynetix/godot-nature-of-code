using Godot;
using Forces;

/// <summary>
/// Particle-related primitives.
/// </summary>
namespace Particles
{
  /// <summary>
  /// Simple particle, based on mover.
  /// </summary>
  public class SimpleParticle : SimpleMover
  {
    /// <summary>Lifespan</summary>
    public float Lifespan = 2;
    /// <summary>Lifespan as alpha</summary>
    public bool LifespanAsAlpha = true;
    /// <summary>Initial offset</summary>
    public Vector2 InitialOffset = Vector2.Zero;

    private float initialLifespan;

    /// <summary>
    /// Create a default non-wrapping particle.
    /// </summary>
    public SimpleParticle()
    {
      WrapMode = WrapModeEnum.None;
    }

    /// <summary>
    /// Check if the particle is dead.
    /// </summary>
    /// <returns>True/False</returns>
    public bool IsDead()
    {
      return Lifespan <= 0;
    }

    /// <summary>
    /// Get lifespan alpha value.
    /// </summary>
    /// <returns>Alpha byte value</returns>
    protected byte GetLifespanAlphaValue()
    {
      return (byte)Mathf.Clamp(((Lifespan / initialLifespan) * 255), 0, 255);
    }

    public override void _Ready()
    {
      base._Ready();
      initialLifespan = Lifespan;
    }

    public override void _Process(float delta)
    {
      if (IsDead())
      {
        return;
      }

      base._Process(delta);

      Lifespan -= delta;

      if (LifespanAsAlpha)
      {
        var alpha = GetLifespanAlphaValue();
        Mesh.Modulate = Mesh.Modulate.WithAlpha(alpha);
      }
    }
  }
}
