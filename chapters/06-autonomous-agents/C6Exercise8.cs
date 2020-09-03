using Godot;
using Agents;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.8: Image Flow Field.
  /// </summary>
  /// Uses ImageFlowField with a simple texture.
  public class C6Exercise8 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.8:\nImage Flow Field";
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var flowField = new ImageFlowField();
      flowField.Resolution = 30;
      flowField.CenterOnScreen = true;
      flowField.SourceTexture = (Texture)GD.Load("res://assets/textures/sample-texture.png");
      AddChild(flowField);

      var vehicle = new SimpleVehicle();
      vehicle.TargetFlow = flowField;
      vehicle.Position = new Vector2(10, size.y / 2);
      vehicle.Velocity = Vector2.Right * vehicle.MaxVelocity;
      AddChild(vehicle);
    }
  }
}
