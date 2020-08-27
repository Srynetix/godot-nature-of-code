using Godot;

public class SimpleAttractor : SimpleCircleSprite
{
  public float Mass = 20.0f;
  public float Gravitation = 1.0f;
  public float MinForce = 5;
  public float MaxForce = 25;

  public SimpleAttractor()
  {
    Radius = 20f;
    BaseColor = Colors.LightGoldenrod;
    Drawing = true;
  }

  public virtual Vector2 Attract(SimpleMover mover)
  {
    var force = GlobalPosition - mover.GlobalPosition;
    var length = Mathf.Clamp(force.Length(), MinForce, MaxForce);
    float strength = (Gravitation * Mass * mover.Mass) / (length * length);
    return force.Normalized() * strength;
  }

  public override void _Ready()
  {
    base._Ready();
    AddToGroup("attractors");
  }

  protected void AttractNodes()
  {
    // For each mover
    foreach (var n in GetTree().GetNodesInGroup("movers"))
    {
      // Ignore parent mover
      var parent = GetParent();
      if (parent != null && parent == n)
      {
        continue;
      }

      var mover = (SimpleMover)n;
      var force = Attract(mover);
      mover.ApplyForce(force);
    }
  }

  public override void _Process(float delta)
  {
    AttractNodes();
  }
}
