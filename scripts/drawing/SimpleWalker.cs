using Godot;

namespace Drawing
{
  /// <summary>
  /// Simple random walker.
  /// </summary>
  public class SimpleWalker : Node2D
  {
    /// <summary>X position</summary>
    public float x;
    /// <summary>Y position</summary>
    public float y;
    /// <summary>Step size</summary>
    public float StepSize;

    protected RandomNumberGenerator generator;
    protected OpenSimplexNoise noise;
    protected float tx;
    protected float ty;

    /// <summary>
    /// Create a default walker.
    /// </summary>
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

    /// <summary>
    /// Update X and Y position from vector.
    /// </summary>
    /// <param name="position">New position</param>
    public void SetXY(Vector2 position)
    {
      x = position.x;
      y = position.y;
    }

    /// <summary>
    /// Update X and Y position from x and y cooordinates.
    /// </summary>
    /// <param name="tx">New X position</param>
    /// <param name="ty">New Y position</param>
    public void SetXY(float tx, float ty)
    {
      x = tx;
      y = ty;
    }

    /// <summary>
    /// Get current step rect.
    /// </summary>
    /// <returns>Rect</returns>
    public Rect2 GetStepRect()
    {
      return new Rect2(x, y, StepSize, StepSize);
    }

    /// <summary>
    /// Compute walker step.
    /// </summary>
    protected virtual void Step() { }

    #region Lifecycle methods

    public override void _Process(float delta)
    {
      Position = new Vector2(x, y);

      Step();
      Update();
    }

    #endregion
  }
}
