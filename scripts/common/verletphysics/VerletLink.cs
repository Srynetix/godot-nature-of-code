using Godot;

namespace VerletPhysics
{
  public class VerletLink : Node2D
  {
    public float RestingDistance = 100;
    public float Stiffness = 1;
    public float TearSensitivity = 200;
    public VerletPoint A;
    public VerletPoint B;
    public bool Drawing = true;

    private VerletWorld world;

    public VerletLink(VerletWorld world, VerletPoint a, VerletPoint b)
    {
      A = a;
      B = b;
      this.world = world;
    }

    public override void _Draw()
    {
      if (Drawing)
      {
        DrawLine(A.GlobalPosition, B.GlobalPosition, Colors.Gray, 2);
      }
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public void Constraint()
    {
      var diff = A.GlobalPosition - B.GlobalPosition;
      var d = diff.Length();
      var difference = (RestingDistance - d) / d;

      if (TearSensitivity != -1 && d > TearSensitivity)
      {
        world.QueueLinkRemoval(this);
      }

      var imA = 1 / A.Mass;
      var imB = 1 / B.Mass;
      var scalarA = (imA / (imA + imB)) * Stiffness;
      var scalarB = Stiffness - scalarA;

      A.GlobalPosition += diff * scalarA * difference;
      B.GlobalPosition -= diff * scalarB * difference;
    }
  }
}
