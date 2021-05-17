using Godot;
using System.Collections.Generic;
using Drawing;

namespace Oscillation
{
    /// <summary>
    /// Simple wave.
    /// </summary>
    public class SimpleWave : Node2D
    {
        private class WaveComponent : SimpleCircleSprite
        {
            public WaveComponent()
            {
                Radius = 30;
                Modulate = Colors.LightBlue.WithAlpha(200);
            }
        }

        /// <summary>Component radius</summary>
        public float Radius = 30;

        /// <summary>Component separation</summary>
        public int Separation = 24;

        /// <summary>Wave angular velocity</summary>
        public float AngularVelocity = 0.1f;

        /// <summary>Wave start angle</summary>
        public float StartAngle;

        /// <summary>Wave start angle update factor</summary>
        public float StartAngleFactor = 1;

        /// <summary>Wave length</summary>
        public float Length = 300;

        /// <summary>Wave amplitude</summary>
        public float Amplitude = 100;

        private readonly List<WaveComponent> components;

        /// <summary>
        /// Create a default wave.
        /// </summary>
        public SimpleWave()
        {
            components = new List<WaveComponent>();
        }

        /// <summary>
        /// Compute Y coordinate from angle.
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Y coordinate</returns>
        protected virtual float ComputeY(float angle)
        {
            return MathUtils.Map(Mathf.Sin(angle), -1, 1, -Amplitude, Amplitude);
        }

        public override void _Ready()
        {
            float angle = StartAngle;

            // Create components
            for (float x = -Length / 2; x <= Length / 2; x += Separation)
            {
                var target = new Vector2(x, ComputeY(angle));
                var node = new WaveComponent();
                AddChild(node);

                node.GlobalPosition = GlobalPosition + target;
                angle += AngularVelocity;
                components.Add(node);
            }
        }

        public override void _Process(float delta)
        {
            StartAngle += delta * StartAngleFactor;
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            float angle = StartAngle;
            foreach (WaveComponent component in components)
            {
                component.GlobalPosition = new Vector2(component.GlobalPosition.x, GlobalPosition.y + ComputeY(angle));
                angle += AngularVelocity;
            }
        }
    }
}
