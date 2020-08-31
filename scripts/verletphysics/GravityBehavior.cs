using Godot;

namespace VerletPhysics
{
  public class GravityBehavior : IBehavior
  {
    public Vector2 Gravity = new Vector2(0, 9.81f);

    public GravityBehavior(Vector2? gravity = null)
    {
      Gravity = gravity ?? Gravity;
    }

    public void ApplyBehavior(VerletPoint point, float delta)
    {
      if (!point.Touched)
      {
        point.ApplyForce(Gravity * point.GravityScale);
      }
    }
  }
}
