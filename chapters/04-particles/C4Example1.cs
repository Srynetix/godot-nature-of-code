using Godot;
using Particles;

namespace Examples
{
  /// <summary>
  /// Chapter 4 - Particle Systems.
  /// </summary>
  namespace Chapter4
  {
    /// <summary>
    /// Example 4.1 - Simple Particle.
    /// </summary>
    /// Uses SimpleParticle with a custom acceleration.
    public class C4Example1 : Node2D, IExample
    {
      public string GetSummary()
      {
        return "Example 4.1:\n"
          + "Simple Particle";
      }

      private class EParticle : SimpleParticle
      {
        protected override void UpdateAcceleration()
        {
          Acceleration = new Vector2(0, 0.01f);
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var particle = new EParticle {
          Position = size / 2
        };
        AddChild(particle);
      }
    }
  }
}
