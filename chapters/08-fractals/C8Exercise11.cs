using Godot;
using VerletPhysics;
using System.Collections.Generic;

namespace Examples
{
    namespace Chapter8
    {
        public class C8Exercise11 : Node2D, IExample
        {
            public string GetSummary()
            {
                return "Exercise 8.11:\nVerlet tree";
            }

            public override void _Ready()
            {
                var size = GetViewportRect().Size;
                var rootPosition = new Vector2(size.x / 2, size.y / 1.15f);

                var physics = new VerletWorld();
                AddChild(physics);

                new VerletTree(physics, rootPosition, Mathf.Pi / 6, 125, 4);
            }

            private class VerletTree
            {
                public VerletTree(VerletWorld world, Vector2 rootPosition, float angle, float length, int generations)
                {
                    var pointSize = 10f;
                    var branchSize = 10f;

                    var root = world.CreatePoint(rootPosition, radius: pointSize, color: Colors.LightPink);
                    var rootTop = world.CreatePoint(rootPosition + new Vector2(0, -length), radius: pointSize, color: Colors.LightPink);
                    var rootLink = world.CreateLink(root, rootTop, restingDistance: length, width: branchSize);

                    // Fix tree root
                    rootTop.PinToCurrentPosition();
                    root.PinToCurrentPosition();

                    var lastLevel = new List<VerletLink>();
                    lastLevel.Add(rootLink);

                    for (var i = 0; i < generations; ++i)
                    {
                        var newLevel = new List<VerletLink>();
                        pointSize *= 0.66f;
                        branchSize *= 0.66f;

                        foreach (var link in lastLevel)
                        {
                            var newLength = (link.B.Position - link.A.Position) * 0.66f;
                            var leftPointTarget = link.B.Position + newLength.Rotated(angle);
                            var rightPointTarget = link.B.Position + newLength.Rotated(-angle);
                            var leftPoint = world.CreatePoint(leftPointTarget, radius: pointSize, color: Colors.LightPink);
                            var rightPoint = world.CreatePoint(rightPointTarget, radius: pointSize, color: Colors.LightPink);
                            var leftLink = world.CreateLink(link.B, leftPoint, restingDistance: newLength.Length(), width: branchSize, tearSensitivityFactor: 2f);
                            var rightLink = world.CreateLink(link.B, rightPoint, restingDistance: newLength.Length(), width: branchSize, tearSensitivityFactor: 2f);

                            newLevel.Add(leftLink);
                            newLevel.Add(rightLink);
                        }

                        lastLevel = newLevel;
                    }
                }
            }
        }
    }
}
