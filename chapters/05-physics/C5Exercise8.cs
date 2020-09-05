using Godot;
using Drawing;

namespace Examples.Chapter5
{
  /// <summary>
  /// Exercise 5.8 - Perlin Mouse Joint.
  /// </summary>
  /// Uses a custom SimpleMouseJoint implementation following a perlin noise 2D curve.
  public class C5Exercise8 : Node2D, IExample
  {
    public string _Summary()
    {
      return "Exercise 5.8:\n"
        + "Perlin Mouse Joint";
    }

    public class PerlinWaveDrawing : Node2D
    {
      public float Length = 500;
      public float Amplitude = 200;
      public float XSpeed = 2;
      public float TimeSpeed = 1;

      public float CurrentX;
      public float CurrentY;

      private OpenSimplexNoise noise;
      private float t;

      public PerlinWaveDrawing()
      {
        noise = new OpenSimplexNoise();
      }

      public override void _Ready()
      {
        float prevX = 0;
        float prevY = 0;
        float curX = 0;
        float curY = 0;
        float curT = 0;

        for (float i = 0; i < Length; i += XSpeed)
        {
          curX = i;
          curY = MathUtils.Map(noise.GetNoise1d(curT), -1, 1, -Amplitude, Amplitude);
          curT += TimeSpeed;

          var lineSprite = new SimpleLineSprite();
          lineSprite.PositionA = GlobalPosition + new Vector2(prevX, prevY);
          lineSprite.PositionB = GlobalPosition + new Vector2(curX, curY);
          lineSprite.Modulate = Colors.Gray;
          lineSprite.Width = 2;
          AddChild(lineSprite);

          prevX = curX;
          prevY = curY;
        }
      }

      public override void _Process(float delta)
      {
        CurrentX += XSpeed;
        CurrentY = MathUtils.Map(noise.GetNoise1d(t), -1, 1, -Amplitude, Amplitude);

        if (CurrentX > Length)
        {
          CurrentX = 0;
          t = 0;
        }

        t += TimeSpeed;
      }
    }

    public class PerlinMouseJoint : Physics.SimpleMouseJoint
    {
      public PerlinWaveDrawing Drawing = null;

      protected override Vector2 ComputeTargetPosition()
      {
        return Drawing.GlobalPosition + new Vector2(Drawing.CurrentX, Drawing.CurrentY);
      }

      public override bool IsActive()
      {
        return true;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      var d = new PerlinWaveDrawing();
      d.Length = size.x / 1.25f;
      d.Position = size / 2 - new Vector2(d.Length / 2, 0);
      AddChild(d);

      // Top left
      var box = new Physics.SimpleBox();
      box.Position = new Vector2(10, 10);
      AddChild(box);

      // Joint
      var joint = new PerlinMouseJoint();
      joint.Drawing = d;
      box.AddChild(joint);
    }
  }
}
