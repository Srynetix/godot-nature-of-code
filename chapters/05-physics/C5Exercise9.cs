using Godot;
using Utils;

namespace Examples
{
  namespace Chapter5
  {
    /// <summary>
    /// Exercise 5.9 - Perlin Kinematic.
    /// </summary>
    /// Same principle as Exercise 5.8, but using kinematic forces instead of a mouse joint.
    public class C5Exercise9 : Node2D, IExample
    {
      public string _Summary()
      {
        return "Exercise 5.9:\n"
          + "Perlin Kinematic\n\n"
          + "Touch screen to spawn more boxes";
      }

      private class PerlinKinematicBox : KinematicBody2D
      {
        public float InitialTValue = 0;
        public C5Exercise8.PerlinWaveDrawing Drawing;

        public float OutlineWidth = 2;
        public Color OutlineColor = Colors.LightBlue;
        public Color BaseColor = Colors.White;
        public Vector2 MeshSize
        {
          get => bodySize;
          set
          {
            bodySize = value;
            if (rectangleShape2D != null)
            {
              rectangleShape2D.Extents = bodySize;
            }
          }
        }

        private Vector2 bodySize = new Vector2(20, 20);
        private CollisionShape2D collisionShape2D;
        private RectangleShape2D rectangleShape2D;

        public override void _Ready()
        {
          rectangleShape2D = new RectangleShape2D();
          rectangleShape2D.Extents = bodySize / 2;
          collisionShape2D = new CollisionShape2D();
          collisionShape2D.Shape = rectangleShape2D;
          AddChild(collisionShape2D);
        }

        public override void _Draw()
        {
          var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
          DrawRect(new Rect2(-bodySize / 2, bodySize), OutlineColor);
          DrawRect(new Rect2(-bodySize / 2 + outlineVec / 2, bodySize - outlineVec / 2), BaseColor);

          DrawLine(Vector2.Zero, (ComputeTargetPosition() - GlobalPosition).Rotated(-GlobalRotation), Colors.Gray, 2);
        }

        private Vector2 ComputeTargetPosition()
        {
          return Drawing.GlobalPosition + new Vector2(Drawing.CurrentX, Drawing.CurrentY);
        }

        public override void _PhysicsProcess(float delta)
        {
          var speed = 2;
          var r = ComputeTargetPosition() - GlobalPosition;

          MoveAndSlide(r * speed);

          Update();
        }
      }

      public override void _Ready()
      {
        var size = GetViewportRect().Size;
        var d = new C5Exercise8.PerlinWaveDrawing();
        d.Length = size.x / 1.25f;
        d.Position = size / 2 - new Vector2(d.Length / 2, 0);
        AddChild(d);

        var spawner = new SimpleTouchSpawner();
        spawner.SpawnFunction = (position) =>
        {
          var box = new PerlinKinematicBox();
          box.Drawing = d;
          box.MeshSize = new Vector2(20, 20);
          box.Position = position;
          return box;
        };
        AddChild(spawner);

        spawner.SpawnBody(new Vector2(10, 10));
      }
    }
  }
}
