using Godot;
using System.Collections.Generic;

namespace Fractals
{
    public class RecursiveBranch : Resource
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public float Weight { get; }

        public RecursiveBranch(Vector2 start, Vector2 end, float weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }

        public void Draw(CanvasItem canvas)
        {
            canvas.DrawLine(Start, End, Colors.White, Weight);
        }
    }

    public class RecursiveTree : Resource
    {
        private readonly List<List<RecursiveBranch>> _branches = new List<List<RecursiveBranch>>();
        private readonly int _generations;
        private readonly float _angle;

        public int Count => _branches.Count;

        public RecursiveTree(RecursiveBranch root, float angle, int generations)
        {
            _angle = angle;
            _generations = generations;

            _branches.Add(new List<RecursiveBranch>() {
                root
            });
        }

        public void GenerateOne()
        {
            var next = new List<RecursiveBranch>();

            foreach (var branch in _branches[^1])
            {
                var newLength = (branch.End - branch.Start) * 0.66f;
                var newWeight = branch.Weight * 0.66f;
                var leftPosition = branch.End + newLength.Rotated(_angle);
                var rightPosition = branch.End + newLength.Rotated(-_angle);

                var leftBranch = new RecursiveBranch(branch.End, leftPosition, newWeight);
                var rightBranch = new RecursiveBranch(branch.End, rightPosition, newWeight);
                next.Add(leftBranch);
                next.Add(rightBranch);
            }

            _branches.Add(next);
        }

        public void GenerateAll()
        {
            for (var i = 0; i < _generations; ++i)
            {
                GenerateOne();
            }
        }

        public void Draw(CanvasItem canvas)
        {
            foreach (var level in _branches)
            {
                foreach (var branch in level)
                {
                    branch.Draw(canvas);
                }
            }
        }

        public void DrawUntil(CanvasItem canvas, int index)
        {
            var limit = Mathf.Max(0, Mathf.Min(index, _branches.Count));
            for (var i = 0; i < limit; ++i)
            {
                foreach (var branch in _branches[i])
                    branch.Draw(canvas);
            }
        }
    }

    public class RecursiveTreeNode : Node2D
    {
        public bool Animated;
        public RecursiveTree RecursiveTree;

        private int _currentLevelIdx = 1;
        private int _levelCount;
        private Timer _timer;

        public override void _Ready()
        {
            RecursiveTree.GenerateAll();
            _levelCount = RecursiveTree.Count;

            _timer = new Timer();
            _timer.WaitTime = 0.5f;
            _timer.Autostart = false;
            _timer.OneShot = false;
            _timer.Connect("timeout", this, nameof(OnTimerTimeout));
            AddChild(_timer);

            if (Animated)
            {
                _timer.Start();
            }
        }

        public override void _Draw()
        {
            if (!Animated)
            {
                RecursiveTree.Draw(this);
            }
            else
            {
                RecursiveTree.DrawUntil(this, _currentLevelIdx);
            }
        }

        private void OnTimerTimeout()
        {
            Update();

            _currentLevelIdx = 1 + ((_currentLevelIdx + 1) % _levelCount);
        }
    }
}
