using Godot;
using Drawing;
using Forces;

namespace Examples.Chapter3
{
  /// <summary>
  /// Example 3.3 - Pointing towards motion.
  /// </summary>
  /// Update SimpleMover rotation depending on mouse position.
  public class C3Example3 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 3.3:\n"
        + "Pointing towards motion";
    }

    private class Mover : SimpleMover
    {
      public Mover()
      {
        Mesh.MeshType = SimpleMesh.TypeEnum.Square;
        Mesh.MeshSize = new Vector2(80, 40);
        SyncRotationOnVelocity = true;
      }

      protected override void UpdateAcceleration()
      {
        var mousePos = GetViewport().GetMousePosition();
        var dir = (mousePos - Position).Normalized();
        Acceleration = dir * 0.5f;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var mover = new Mover {
        Position = new Vector2((float)GD.RandRange(0, size.x), (float)GD.RandRange(0, size.y))
      };
      AddChild(mover);
    }
  }
}
