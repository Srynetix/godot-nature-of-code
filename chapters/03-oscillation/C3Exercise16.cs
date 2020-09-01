using Godot;
using Forces;
using Oscillation;

namespace Examples
{
  namespace Chapter3
  {
    /// <summary>
    /// Exercise 3.16 - Multiple Springs.
    /// </summary>
    /// Uses a SimpleSpring tree.
    public class C3Exercise16 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 3.16:\n"
          + "Multiple Springs\n\n"
          + "You can move the balls by touching them.";
      }

      private class Mover : SimpleMover
      {
        public Mover() : base(WrapModeEnum.None)
        {
          Mass = 24;
        }

        protected override void UpdateAcceleration()
        {
          // Gravity
          ApplyForce(new Vector2(0, 0.9f));
          ApplyDamping(0.98f);
        }
      }

      public int SpringCount = 4;
      public int SpringPerSpring = 3;
      public int SpringSeparation = 30;
      public float SprintChildSurfaceRatio = 8;

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var springStep = size.x / SpringCount;
        var springPerSpringSurface = size.x / SprintChildSurfaceRatio;
        var springPerSpringOffset = springPerSpringSurface / SpringPerSpring;

        for (int x = 0; x < SpringCount; ++x)
        {
          var spring = new SimpleSpring();
          var mover = new Mover();

          spring.Length = size.y / 4 + x * SpringSeparation;
          spring.MaxLength = size.y / 2;
          spring.MinLength = size.y / 8;
          spring.Position = new Vector2(springStep / 2 + springStep * x, 0);

          spring.SetMover(mover, new Vector2(0, size.y / 4));
          AddChild(spring);

          for (int s = 0; s < SpringPerSpring; ++s)
          {
            var cSpring = new SimpleSpring();
            cSpring.ShowBehindParent = true;
            cSpring.Length = size.y / 4 + s * SpringSeparation;
            cSpring.MaxLength = size.y / 2;
            cSpring.MinLength = size.y / 8;
            cSpring.Position = Vector2.Zero;

            cSpring.SetMover(new Mover(), new Vector2(springPerSpringOffset * s - springPerSpringSurface / 2 + springPerSpringOffset / 2, size.y / 4));
            mover.AddChild(cSpring);
          }
        }
      }
    }
  }
}
