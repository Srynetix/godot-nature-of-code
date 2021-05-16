using Godot;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Example7 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 8.7:\nRecursive tree with mouse\n\nMove from left to right to change tree rotation amount.";
            }

            private float _baseAngle = Mathf.Pi / 6;
            private bool _mouseUpdated;

            public override void _Input(InputEvent @event)
            {
                if (@event is InputEventMouse eventMouse)
                {
                    var size = GetViewportRect().Size;
                    _baseAngle = MathUtils.Map(eventMouse.Position.x, 0, size.x, 0, Mathf.Pi);
                    _mouseUpdated = true;
                }
            }

            public override void _Process(float delta)
            {
                if (_mouseUpdated)
                {
                    Update();
                    _mouseUpdated = false;
                }
            }

            public override void _Draw()
            {
                var size = GetViewportRect().Size;
                DrawTree(new Vector2(size.x / 2, size.y / 1.15f), 0, 125);
            }

            private void DrawTree(Vector2 position, float rotation, float length)
            {
                if (length <= 2)
                {
                    return;
                }

                var start = position;
                var end = position + new Vector2(0, -length).Rotated(rotation);
                var newRotation = _baseAngle;
                var newLength = length * 0.66f;

                DrawLine(start, end, Colors.White);
                DrawTree(end, rotation + newRotation, newLength);
                DrawTree(end, rotation - newRotation, newLength);
            }
        }
    }
}
