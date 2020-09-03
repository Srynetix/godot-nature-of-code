using Godot;
using Agents;

namespace Examples.Chapter6
{
  /// <summary>
  /// Exercise 6.7: Animated Flow Field.
  /// </summary>
  /// Uses a local 'z' variable with GetNoise3d. 
  public class C6Exercise7 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 6.7:\nAnimated Flow Field";
    }

    private class AnimatedFlowField : SimpleFlowField
    {
      private OpenSimplexNoise noise;
      private float z = 0;

      public AnimatedFlowField()
      {
        noise = new OpenSimplexNoise();
      }

      protected override Vector2 ComputeDirectionFromPosition(int x, int y)
      {
        var f = MathUtils.Map(noise.GetNoise3d(x * 8, y * 8, z * 8), 0, 1, 0, Mathf.Pi * 2);
        return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      }

      public override void _Process(float delta)
      {
        base._Process(delta);

        RefreshDirections();
        z += 0.1f;
      }
    }

    #region Lifecycle methods

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var field = new AnimatedFlowField();
      field.Resolution = 30;
      AddChild(field);

      var vehicle = new SimpleVehicle();
      vehicle.TargetFlow = field;
      vehicle.Position = size / 2;
      AddChild(vehicle);
    }

    #endregion
  }
}
