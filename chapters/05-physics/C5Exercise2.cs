using Godot;
using Utils;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Exercise 5.2 - Anti-Gravity Boxes.
    /// </summary>
    /// Custom SimpleBox without gravity.
    public class C5Exercise2 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 5.2:\n"
          + "Anti-Gravity Boxes\n\n"
          + "Touch screen to spawn boxes";
      }

      private class AntigravityBox : Physics.SimpleBox
      {
        public override void _Ready()
        {
          base._Ready();

          // Remove gravity
          GravityScale = 0;
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        var spawner = new SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var box = new AntigravityBox();
          box.BodySize = new Vector2(20, 20);
          box.GlobalPosition = position;
          return box;
        };
        AddChild(spawner);

        int boxCount = 10;
        for (int i = 0; i < boxCount; ++i)
        {
          spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
        }
      }
    }
  }
}
