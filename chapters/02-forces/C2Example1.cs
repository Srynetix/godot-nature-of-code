using Godot;
using Forces;

namespace Examples
{
    /// <summary>
    /// Chapter 2 - Forces.
    /// </summary>
    namespace Chapter2
    {
        /// <summary>
        /// Example 2.1 - Forces.
        /// </summary>
        /// Uses force methods from SimpleMover to apply forces as gravity or wind.
        public class C2Example1 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Example 2.1:\n"
                  + "Forces";
            }

            private class Mover : SimpleMover
            {
                public Mover() : base(WrapModeEnum.Bounce) { }

                protected override void UpdateAcceleration()
                {
                    var wind = new Vector2(0.01f, 0);
                    var gravity = new Vector2(0, 0.09f);

                    ApplyForce(wind);
                    ApplyForce(gravity);
                }
            }

            public override void _Ready()
            {
                var mover = new Mover
                {
                    Position = GetViewportRect().Size / 2
                };

                AddChild(mover);
            }
        }
    }
}
