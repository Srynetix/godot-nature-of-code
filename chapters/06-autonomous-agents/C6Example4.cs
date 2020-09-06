using Godot;
using Agents;

namespace Examples.Chapter6
{
  /// <summary>
  /// Example 6.4: Flow Field Following.
  /// </summary>
  /// Uses SimpleFlowField with a Perlin noise.
  public class C6Example4 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Example 6.4:\nFlow Field Following";
    }

    private class CustomFlowField : SimpleFlowField
    {
      private readonly OpenSimplexNoise noise;

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

    public override void _Ready()
    {
      var size = GetViewportRect().Size;

      var field = new CustomFlowField { Resolution = 30 };
      AddChild(field);

      var vehicle = new SimpleVehicle {
        TargetFlow = field,
        Position = size / 2
      };
      AddChild(vehicle);
    }
  }
}
