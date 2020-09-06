using Godot;
using Utils;
using Physics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Exercise 5.3 - Sine/Perlin Boundaries.
  /// </summary>
  /// Use SimpleStaticLines to define a sine and a perlin pattern.
  public class C5Exercise3 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 5.3:\n"
        + "Sine/Perlin Boundaries\n\n"
        + "Touch screen to spawn balls";
    }

    public class WaveWall : SimpleStaticLines
    {
      public float Frequency = 1f;
      public float Amplitude = 100f;
      public float YOffset = 100f;
      public float Separation = 10f;
      public float Length = 100f;

      public virtual float ComputeY(float t)
      {
        return Mathf.Sin(t);
      }

      public override void _Ready()
      {
        float t = 0;

        var otherPoint = new Vector2(-Length / 2, MathUtils.Map(ComputeY(t), -1, 1, -Amplitude, Amplitude) + YOffset);
        for (float x = -Length / 2; x <= Length / 2; x += Separation)
        {
          var yCoord = MathUtils.Map(ComputeY(t), -1, 1, -Amplitude, Amplitude);
          var point = new Vector2(x, yCoord + YOffset);

          AddSegment(otherPoint, point);
          otherPoint = point;
          t += Frequency;
        }
      }
    }

    public class SineWall : WaveWall
    {
      public SineWall()
      {
        Frequency = 0.2f;
      }

      public override float ComputeY(float t)
      {
        return Mathf.Sin(t);
      }
    }

    public class PerlinWall : WaveWall
    {
      private readonly OpenSimplexNoise noise;

      public PerlinWall()
      {
        noise = new OpenSimplexNoise();
        Amplitude = 200f;
        Frequency = 8f;
      }

      public override float ComputeY(float t)
      {
        return noise.GetNoise1d(t);
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var leftWall = new SineWall {
        Position = new Vector2(size.x / 4, size.y / 2),
        Length = size.x / 2
      };
      AddChild(leftWall);

      var rightWall = new PerlinWall {
        Position = new Vector2(size.x / 1.25f, size.y / 2),
        Length = size.x / 2
      };
      AddChild(rightWall);

      var spawner = new SimpleTouchSpawner {
        SpawnFunction = (position) =>
        {
          return new SimpleBall {
            GlobalPosition = position
          };
        }
      };
      AddChild(spawner);

      const int ballCount = 10;
      for (int i = 0; i < ballCount; ++i)
      {
        spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, (size.y / 2) - 100));
      }
    }
  }
}
