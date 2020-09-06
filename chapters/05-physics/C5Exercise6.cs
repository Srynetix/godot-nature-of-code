using Godot;
using System.Collections.Generic;
using Drawing;
using Utils;
using Physics;

namespace Examples.Chapter5
{
  /// <summary>
  /// Exercise 5.6 - Bridge.
  /// </summary>
  /// Simple bridge using multiple physics body types.
  public class C5Exercise6 : Node2D, IExample
  {
    public string GetSummary()
    {
      return "Exercise 5.6:\n"
        + "Bridge\n\n"
        + "Touch screen to spawn balls";
    }

    private class ChainAnchor : StaticBody2D
    {
      public float Radius = 10;

      private CollisionShape2D collisionShape2D;
      private CircleShape2D circleShape2D;
      private SimpleCircleSprite sprite;

      public override void _Ready()
      {
        circleShape2D = new CircleShape2D { Radius = Radius };
        collisionShape2D = new CollisionShape2D { Shape = circleShape2D };
        AddChild(collisionShape2D);

        sprite = new SimpleCircleSprite
        {
          Radius = Radius,
          Modulate = Colors.LightGoldenrod
        };
        AddChild(sprite);
      }

      public void LinkTarget(PhysicsBody2D target, float softness = 0, float bias = 0)
      {
        var pinJoint = new PinJoint2D
        {
          NodeA = GetPath(),
          NodeB = target.GetPath(),
          Softness = softness,
          Bias = bias
        };
        AddChild(pinJoint);
      }
    }

    private class ChainLink : RigidBody2D
    {
      public float Radius = 10;

      private CollisionShape2D collisionShape2D;
      private CircleShape2D circleShape2D;
      private SimpleCircleSprite sprite;

      public override void _Ready()
      {
        circleShape2D = new CircleShape2D { Radius = Radius };
        collisionShape2D = new CollisionShape2D { Shape = circleShape2D };
        AddChild(collisionShape2D);

        sprite = new SimpleCircleSprite
        {
          Radius = Radius,
          Modulate = Colors.LightGoldenrod
        };
        AddChild(sprite);
      }

      public void LinkToParent(PhysicsBody2D parent, float softness = 0, float bias = 0)
      {
        var pinJoint = new PinJoint2D
        {
          NodeA = parent.GetPath(),
          NodeB = GetPath(),
          Softness = softness,
          Bias = bias
        };
        parent.AddChild(pinJoint);
      }
    }

    private class SimpleChain : Node2D
    {
      // Use multiple PinJoint2D to create a chain
      public Vector2 StartPosition;
      public Vector2 EndPosition;
      public int LinkCount = 10;
      public float Softness = 0;
      public float Bias = 0;

      private ChainAnchor startAnchor;
      private ChainAnchor endAnchor;
      private readonly List<ChainLink> links;
      private readonly List<SimpleLineSprite> lineSprites;
      private readonly Node2D linksContainer;
      private readonly Node2D circleContainer;

      public SimpleChain()
      {
        links = new List<ChainLink>();
        lineSprites = new List<SimpleLineSprite>();
        linksContainer = new Node2D();
        circleContainer = new Node2D();
      }

      public override void _Ready()
      {
        var width = (EndPosition.x - StartPosition.x) / (LinkCount + 1);
        var height = (EndPosition.y - StartPosition.y) / (LinkCount + 1);

        AddChild(linksContainer);
        AddChild(circleContainer);

        startAnchor = new ChainAnchor { Position = StartPosition };
        circleContainer.AddChild(startAnchor);

        PhysicsBody2D lastLink = startAnchor;
        for (int i = 0; i < LinkCount; ++i)
        {
          var link = new ChainLink
          {
            Position = StartPosition + new Vector2(width + (i * width), height + (i * height))
          };
          linksContainer.AddChild(link);
          link.LinkToParent(lastLink, Softness, Bias);
          links.Add(link);

          var lineSprite = new SimpleLineSprite();
          circleContainer.AddChild(lineSprite);
          lineSprite.PositionA = lastLink.Position;
          lineSprite.PositionB = link.Position;
          lineSprites.Add(lineSprite);

          lastLink = link;
        }

        endAnchor = new ChainAnchor
        {
          Position = EndPosition
        };
        circleContainer.AddChild(endAnchor);
        endAnchor.LinkTarget(lastLink, Softness, Bias);

        var lastLineSprite = new SimpleLineSprite();
        linksContainer.AddChild(lastLineSprite);
        lastLineSprite.PositionA = lastLink.Position;
        lastLineSprite.PositionB = endAnchor.Position;
        lineSprites.Add(lastLineSprite);
      }

      public override void _Process(float delta)
      {
        PhysicsBody2D lastLink = startAnchor;

        for (int i = 0; i < links.Count; ++i)
        {
          var lineSprite = lineSprites[i];
          lineSprite.PositionA = lastLink.Position;
          lineSprite.PositionB = links[i].Position;
          lastLink = links[i];
        }

        var lastLineSprite = lineSprites[lineSprites.Count - 1];
        lastLineSprite.PositionA = lastLink.Position;
        lastLineSprite.PositionB = endAnchor.Position;
      }
    }

    public override void _Ready()
    {
      var size = GetViewportRect().Size;
      const int offset = 50;

      var chain = new SimpleChain
      {
        Softness = 0.1f,
        Bias = 0f,
        LinkCount = 30,
        StartPosition = new Vector2(offset, size.y / 2),
        EndPosition = new Vector2(size.x - offset, size.y / 2)
      };
      AddChild(chain);

      var spawner = new SimpleTouchSpawner
      {
        SpawnFunction = (position) =>
        {
          return new SimpleBall()
          {
            Radius = 15,
            GlobalPosition = position
          };
        }
      };
      AddChild(spawner);

      // Spawn balls
      const int bodyCount = 10;
      var bodyLength = size.x / 2 / bodyCount;
      for (int i = 0; i < bodyCount; ++i)
      {
        var x = (size.x / 4) + (i * bodyLength);
        var y = size.y / 8;

        spawner.SpawnBody(new Vector2(x, y));
      }
    }
  }
}
