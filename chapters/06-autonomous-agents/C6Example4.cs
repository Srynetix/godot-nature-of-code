using Godot;
using Forces;

namespace Examples
{
  namespace Chapter6
  {
    /// <summary>
    /// Example 6.4: Flow Field Following.
    /// </summary>
    /// Uses SimpleFlowField with a Perlin noise.
    public class C6Example4 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Example 6.4:\nFlow Field Following";
      }

      private class CustomFlowField : SimpleFlowField
      {
        private OpenSimplexNoise noise;

        public CustomFlowField()
        {
          noise = new OpenSimplexNoise();
        }

        protected override Vector2 ComputeDirectionFromPosition(int x, int y)
        {
          var f = MathUtils.Map(noise.GetNoise2d(x * 8, y * 8), 0, 1, 0, Mathf.Pi * 2);
          return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
        }
      }

      #region Lifecycle methods

      public override void _Ready()
      {
        var size = GetViewportRect().Size;

        var field = new CustomFlowField();
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
}