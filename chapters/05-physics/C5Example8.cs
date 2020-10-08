using Godot;
using Physics;

namespace Examples.Chapter5
{
    /// <summary>
    /// Example 5.8 - Mouse Joint.
    /// </summary>
    /// Uses SimpleMouseJoint to simulate a mouse joint.
    public class C5Example8 : Node2D, IExample
    {
        public string GetSummary()
        {
            return "Example 5.8:\n"
              + "Mouse Joint\n\n"
              + "Touch screen to move box";
        }

        public override void _Ready()
        {
            var size = GetViewportRect().Size;

            var box = new SimpleBox
            {
                Position = size / 2
            };
            AddChild(box);
            box.AddChild(new SimpleMouseJoint());

            var floor = new SimpleWall
            {
                BodySize = new Vector2(size.x, 100),
                Position = new Vector2(size.x / 2, size.y)
            };
            AddChild(floor);
        }
    }
}
