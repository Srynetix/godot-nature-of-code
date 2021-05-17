using Godot;
using System.Collections.Generic;
using System.Text;

namespace Fractals
{
    /// <summary>
    /// L-System.
    /// </summary>
    public class LSystem
    {
        /// <summary>Current string.</summary>
        public string Current;
        /// <summary>Rules.</summary>
        public List<LSystemRule> Rules;
        /// <summary>Generation count.</summary>
        public int Generation => _generation;

        private int _generation;

        /// <summary>
        /// Generate one generation.
        /// </summary>
        public void GenerateOne()
        {
            _generation++;

            var builder = new StringBuilder();
            for (var i = 0; i < Current.Length; ++i)
            {
                var letter = Current[i];
                builder.Append(ApplyRules(letter));
            }

            Current = builder.ToString();
        }

        /// <summary>
        /// Get matching rules for a letter.
        /// </summary>
        /// <param name="letter">Letter</param>
        /// <returns>Matching rules.</returns>
        protected List<LSystemRule> GetMatchingRules(char letter)
        {
            var rules = new List<LSystemRule>();

            foreach (var rule in Rules)
            {
                if (rule.Input == letter)
                {
                    rules.Add(rule);
                }
            }

            return rules;
        }

        /// <summary>
        /// Apply rules on a letter.
        /// </summary>
        /// <param name="letter">Letter</param>
        /// <returns>Output text.</returns>
        protected virtual string ApplyRules(char letter)
        {
            var matchingRules = GetMatchingRules(letter);
            if (matchingRules.Count > 0)
            {
                return matchingRules[0].Output;
            }

            return letter.ToString();
        }
    }

    /// <summary>
    /// Stochastic L-System.
    /// </summary>
    public class StochasticLSystem : LSystem
    {
        /// <summary>
        /// Apply rules on a letter.
        /// </summary>
        /// <param name="letter">Letter</param>
        /// <returns>Output text.</returns>
        protected override string ApplyRules(char letter)
        {
            var matchingRules = GetMatchingRules(letter);
            if (matchingRules.Count > 0)
            {
                var index = MathUtils.RandRangei(0, matchingRules.Count - 1);
                return matchingRules[index].Output;
            }

            return letter.ToString();
        }
    }

    /// <summary>
    /// L-System rule.
    /// </summary>
    public class LSystemRule
    {
        /// <summary>Input character.</summary>
        public char Input;
        /// <summary>Output string.</summary>
        public string Output;
    }

    /// <summary>
    /// L-System turtle node.
    /// </summary>
    public class LSystemTurtleNode : Node2D
    {
        /// <summary>L-System.</summary>
        public LSystem LSystem;
        /// <summary>Initial length.</summary>
        public float Length = 10;
        /// <summary>Initial rotation.</summary>
        public float InitialRotation;
        /// <summary>Theta value.</summary>
        public float Theta = Mathf.Pi / 6;
        /// <summary>Length multiplicator.</summary>
        public float LengthMultiplicator = 1;

        private Vector2 _drawPosition;
        private float _drawRotation;
        private readonly Stack<Vector2> _lastPositions = new Stack<Vector2>();
        private readonly Stack<float> _lastRotations = new Stack<float>();

        /// <summary>
        /// Generate one generation.
        /// </summary>
        public void GenerateOne()
        {
            LSystem.GenerateOne();
            Length *= LengthMultiplicator;
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
            _drawRotation = InitialRotation;

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
