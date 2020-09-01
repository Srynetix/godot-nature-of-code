using Godot;
using Oscillation;

namespace Examples
{
  namespace Chapter3
  {
    /// <summary>
    /// Example 3.10 - Swinging Pendulum.
    /// </summary>
    /// Uses SimplePendulum.
    public class C3Example10 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 3.10:\n"
          + "Swinging Pendulum\n\n"
          + "You can move the pendulum by touching it.";
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var pendulum = new SimplePendulum();
        pendulum.Angle = Mathf.Pi / 4;
        pendulum.RopeLength = size.y / 2;
        pendulum.Position = new Vector2(size.x / 2, 0);
        AddChild(pendulum);
      }
    }
  }
}
