using Godot;

public class SimpleWalker : Node2D
{
  public float x;
  public float y;
  public float StepSize;

  public RandomNumberGenerator generator;

  public OpenSimplexNoise noise;
  public float tx;
  public float ty;

  public SimpleWalker()
  {
    x = 0;
    y = 0;
    StepSize = 3;

    // Needed from C0Exercise5
    generator = new RandomNumberGenerator();
    generator.Randomize();

    // Needed from C0Exercise7
    noise = new OpenSimplexNoise();
    tx = 0;
    ty = 10000;
  }

  public void SetXY(Vector2 position)
  {
    x = position.x;
    y = position.y;
  }

  public void SetXY(float tx, float ty)
  {
    x = tx;
    y = ty;
  }

  public virtual void Step() { }

  public Rect2 GetStepRect()
  {
    return new Rect2(x, y, StepSize, StepSize);
  }

  public override void _Process(float delta)
  {
    Position = new Vector2(x, y);

    Step();
    Update();
  }
}