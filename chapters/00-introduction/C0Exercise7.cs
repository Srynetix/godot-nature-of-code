using Godot;

public class C0Exercise7 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise I.7:\n"
      + "In the above random walker, the result of the noise function is mapped directly to the Walker’s location.\n"
      + "Create a random walker where you instead map the result of the noise() function to a Walker’s step size.";
  }

  public class Walker : SimpleWalker
  {
    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, 20, Colors.Black);
      DrawCircle(Vector2.Zero, 18, Colors.LightGray);
    }

    public override void Step()
    {
      RandomStep();

      tx += 1f;
      ty += 1f;
    }

    public float ComputeStepSize(float t)
    {
      return Utils.Map(noise.GetNoise1d(t), -1, 1, -1, 1);
    }

    public void RandomStep()
    {
      float stepx = ComputeStepSize(tx);
      float stepy = ComputeStepSize(ty);

      x += stepx;
      y += stepy;
    }
  }

  private Walker walker;

  public override void _Ready()
  {
    GD.Randomize();
    walker = new Walker();
    walker.SetXY(GetViewport().Size / 2);
    AddChild(walker);
  }
}
