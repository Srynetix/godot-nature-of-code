using Godot;
using System.Collections.Generic;
using System.Text;

namespace Fractals
{
    public class LSystem : Resource
    {
        public string Current;
        public List<LSystemRule> Rules;

        public int Generation => _generation;

        private int _generation;

        public void GenerateOne()
        {
            _generation++;

            var builder = new StringBuilder();
            for (var i = 0; i < Current.Length; ++i)
            {
                var letter = Current[i];
                var ruleUsed = false;
                foreach (var rule in Rules)
                {
                    if (rule.Input == letter)
                    {
                        builder.Append(rule.Output);
                        ruleUsed = true;
                        break;
                    }
                }

                if (!ruleUsed)
                {
                    builder.Append(letter);
                }
            }

            Current = builder.ToString();
        }
    }

    public class LSystemRule : Resource
    {
        public char Input;
        public string Output;
    }

    public class LSystemTurtleNode : Node2D
    {
        public LSystem LSystem;
        public float Length = 10;
        public float Theta = Mathf.Pi / 6;

        public Vector2 _drawPosition;
        public float _drawRotation;
        public Stack<Vector2> _lastPositions = new Stack<Vector2>();
        public Stack<float> _lastRotations = new Stack<float>();

        public void GenerateOne()
        {
            LSystem.GenerateOne();
            Length *= 0.85f;
            Update();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
            {
                GenerateOne();
            }
        }

        public override void _Draw()
        {
            _drawPosition = Vector2.Zero;

            foreach (var letter in LSystem.Current)
            {
                if (letter == 'F')
                {
                    var targetPosition = new Vector2(0, -Length).Rotated(_drawRotation);
                    DrawLine(_drawPosition, _drawPosition + targetPosition, Colors.White);
                    _drawPosition += targetPosition;
                }
                else if (letter == 'G')
                {
                    var targetPosition = new Vector2(0, -Length).Rotated(_drawRotation);
                    _drawPosition += targetPosition;
                }
                else if (letter == '-')
                {
                    _drawRotation -= Theta;
                }
                else if (letter == '+')
                {
                    _drawRotation += Theta;
                }
                else if (letter == '[')
                {
                    _lastPositions.Push(new Vector2(_drawPosition.x, _drawPosition.y));
                    _lastRotations.Push(_drawRotation);
                }
                else if (letter == ']')
                {
                    _drawPosition = _lastPositions.Pop();
                    _drawRotation = _lastRotations.Pop();
                }
            }
        }
    }
}
