using Godot;
using Assets;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.9: Angle between two vectors.
  /// </summary>
  /// Uses Godot Vector2.AngleTo method.
  public class C6Exercise9 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.9:\nAngle between two vectors";
    }

    private float length = 100;
    private float width = 4;
    private Vector2 fixedVector;
    private Vector2 movingVector;
    private Font defaultFont;
    private float theta;

    #region Lifecycle methods

    public override void _Ready()
    {
      theta = 0;
      defaultFont = SimpleDefaultFont.LoadDefaultFont();
      fixedVector = Vector2.Right;
      movingVector.x = Mathf.Cos(theta);
      movingVector.y = Mathf.Sin(theta);
    }

    public override void _Process(float delta)
    {
      theta += delta / 2;
      movingVector.x = Mathf.Cos(theta);
      movingVector.y = Mathf.Sin(theta);

      // Loop theta
      while (theta > Mathf.Pi * 2)
      {
        theta -= Mathf.Pi * 2;
      }

      Update();
    }

    public override void _Draw()
    {
      var size = GetViewportRect().Size;
      var angle = fixedVector.AngleTo(movingVector);
      var degText = ((int)Mathf.Rad2Deg(angle)).ToString() + " degrees";
      var radText = angle.ToString() + " radians";

      DrawLine(size / 2, size / 2 + fixedVector * length, Colors.LightBlue, width);
      DrawLine(size / 2, size / 2 + new Vector2(movingVector.x * length, -movingVector.y * length), Colors.LightGreen, width);
      DrawCircle(size / 2, width * 4, Colors.LightBlue.WithAlpha(128));
      DrawString(defaultFont, new Vector2(100, size.y / 2), degText);
      DrawString(defaultFont, new Vector2(100, size.y / 2 + 16), radText);
    }

    #endregion
  }
}
