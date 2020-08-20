using Godot;
using System.Collections.Generic;

public class C5Exercise6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.6:\n"
      + "Bridge\n\n"
      + "Touch screen to spawn balls";
  }

  public class ChainAnchor : StaticBody2D
  {
    public float Radius = 10;

    private CollisionShape2D collisionShape2D;
    private CircleShape2D circleShape2D;

    public override void _Ready()
    {
      circleShape2D = new CircleShape2D();
      circleShape2D.Radius = Radius;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = circleShape2D;
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, Radius, Colors.LightGoldenrod);
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public void LinkTarget(PhysicsBody2D target, float softness = 0, float bias = 0)
    {
      var pinJoint = new PinJoint2D();
      pinJoint.NodeA = GetPath();
      pinJoint.NodeB = target.GetPath();
      pinJoint.Softness = softness;
      pinJoint.Bias = bias;
      AddChild(pinJoint);
    }
  }

  public class ChainLink : RigidBody2D
  {
    public float Radius = 10;

    private CollisionShape2D collisionShape2D;
    private CircleShape2D circleShape2D;

    public override void _Ready()
    {
      circleShape2D = new CircleShape2D();
      circleShape2D.Radius = Radius;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = circleShape2D;
      AddChild(collisionShape2D);
    }

    public void LinkToParent(PhysicsBody2D parent, float softness = 0, float bias = 0)
    {
      var pinJoint = new PinJoint2D();
      pinJoint.NodeA = parent.GetPath();
      pinJoint.NodeB = GetPath();
      pinJoint.Softness = softness;
      pinJoint.Bias = bias;
      parent.AddChild(pinJoint);
    }

    public override void _Draw()
    {
      DrawCircle(Vector2.Zero, Radius, Colors.LightGoldenrod);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }

  public class SimpleChain : Node2D
  {
    // Use multiple PinJoint2D to create a chain
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public int LinkCount = 10;
    public float Softness = 0;
    public float Bias = 0;

    private ChainAnchor startAnchor;
    private ChainAnchor endAnchor;
    private List<ChainLink> links;

    public SimpleChain()
    {
      links = new List<ChainLink>();
    }

    public override void _Ready()
    {
      var width = (EndPosition.x - StartPosition.x) / (LinkCount + 1);
      var height = (EndPosition.y - StartPosition.y) / (LinkCount + 1);

      startAnchor = new ChainAnchor();
      startAnchor.Position = StartPosition;
      AddChild(startAnchor);

      PhysicsBody2D lastLink = startAnchor;
      for (int i = 0; i < LinkCount; ++i)
      {
        var link = new ChainLink();
        link.Position = StartPosition + new Vector2(width + i * width, height + i * height);
        AddChild(link);
        link.LinkToParent(lastLink, Softness, Bias);
        lastLink = link;
        links.Add(link);
      }

      endAnchor = new ChainAnchor();
      endAnchor.Position = EndPosition;
      AddChild(endAnchor);
      endAnchor.LinkTarget(lastLink, Softness, Bias);
    }

    public override void _Draw()
    {
      PhysicsBody2D lastLink = startAnchor;

      for (int i = 0; i < links.Count; ++i)
      {
        DrawLine(lastLink.Position, links[i].Position, Colors.White, 2);
        lastLink = links[i];
      }

      DrawLine(lastLink.Position, endAnchor.Position, Colors.White, 2);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var offset = 50;

    var chain = new SimpleChain();
    chain.Softness = 0.1f;
    chain.Bias = 0f;
    chain.LinkCount = 30;
    chain.StartPosition = new Vector2(offset, size.y / 2);
    chain.EndPosition = new Vector2(size.x - offset, size.y / 2);
    AddChild(chain);

    // Spawn balls
    var bodyCount = 10;
    var bodyLength = (size.x / 2) / bodyCount;
    for (int i = 0; i < bodyCount; ++i)
    {
      var x = size.x / 4 + i * bodyLength;
      var y = size.y / 8;

      SpawnBody(new Vector2(x, y));
    }
  }

  private void SpawnBody(Vector2 position)
  {
    var body = new SimpleBall();
    body.Radius = 20;
    body.GlobalPosition = position;
    AddChild(body);
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        SpawnBody(eventScreenTouch.Position);
      }
    }

    if (@event is InputEventScreenDrag eventScreenDrag)
    {
      SpawnBody(eventScreenDrag.Position);
    }
  }
}
