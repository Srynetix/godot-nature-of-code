using Godot;

public class SimpleRepeller : SimpleAttractor
{
  public override Vector2 Attract(SimpleMover mover)
  {
    return -base.Attract(mover);
  }
}