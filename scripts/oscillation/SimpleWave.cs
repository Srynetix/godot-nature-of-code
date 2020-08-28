using Godot;
using System.Collections.Generic;

public class SimpleWave : Node2D
{
  public class WaveComponent : SimpleCircleSprite
  {
    public WaveComponent()
    {
      Radius = 30;
      Modulate = Colors.LightBlue.WithAlpha(200);
    }
  }

  public float Radius = 30;
  public int Separation = 24;
  public float AngularVelocity = 0.1f;
  public float StartAngle = 0;
  public float StartAngleFactor = 1;
  public float Length = 300;
  public float Amplitude = 100;

  private List<WaveComponent> components;

  public SimpleWave()
  {
    components = new List<WaveComponent>();
  }

  public override void _Ready()
  {
    float angle = StartAngle;

    // Create components
    for (float x = -Length / 2; x <= Length / 2; x += Separation)
    {
      var target = new Vector2(x, ComputeY(angle));
      var node = new WaveComponent();
      AddChild(node);

      node.GlobalPosition = GlobalPosition + target;
      angle += AngularVelocity;
      components.Add(node);
    }
  }

  public virtual float ComputeY(float angle)
  {
    return MathUtils.Map(Mathf.Sin(angle), -1, 1, -Amplitude, Amplitude);
  }

  private void UpdatePositions()
  {
    float angle = StartAngle;
    foreach (WaveComponent component in components)
    {
      component.GlobalPosition = new Vector2(component.GlobalPosition.x, GlobalPosition.y + ComputeY(angle));
      angle += AngularVelocity;
    }
  }

  public override void _Process(float delta)
  {
    StartAngle += delta * StartAngleFactor;
    UpdatePositions();
  }
}
