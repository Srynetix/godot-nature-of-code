using Godot;

public class C5Example5 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 5.5:\n"
      + "Multiple Shapes / One Body\n\n"
      + "Touch screen to spawn bodies";
  }

  public class MultiShapeBody : RigidBody2D
  {
    private CollisionShape2D bodyCollisionShape;
    private RectangleShape2D bodyShape;
    private CollisionShape2D headCollisionShape;
    private CircleShape2D headShape;

    public override void _Ready()
    {
      bodyCollisionShape = new CollisionShape2D();
      bodyShape = new RectangleShape2D();
      bodyShape.Extents = new Vector2(10, 20);
      bodyCollisionShape.Shape = bodyShape;
      AddChild(bodyCollisionShape);

      headCollisionShape = new CollisionShape2D();
      headShape = new CircleShape2D();
      headShape.Radius = 15;
      headCollisionShape.Position = new Vector2(0, -30);
      headCollisionShape.Shape = headShape;
      AddChild(headCollisionShape);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(-bodyShape.Extents, bodyShape.Extents * 2), Colors.White);
      DrawCircle(headCollisionShape.Position, headShape.Radius, Colors.LightBlue);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var floorHeight = 25;
    var offset = 50;

    // Add left floor
    var leftFloor = new SimpleWall();
    leftFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    leftFloor.Position = new Vector2(size.x / 2.5f / 2 + offset, size.y);
    AddChild(leftFloor);

    // Add right floor
    var rightFloor = new SimpleWall();
    rightFloor.BodySize = new Vector2(size.x / 2.5f, floorHeight);
    rightFloor.Position = new Vector2(size.x - size.x / 2.5f / 2 - offset, size.y - offset * 2);
    AddChild(rightFloor);

    int bodyCount = 10;
    for (int i = 0; i < bodyCount; ++i)
    {
      SpawnBody(Utils.RandVector2(offset * 2, size.x - offset * 2, offset * 2, size.y - offset * 2));
    }
  }

  private void SpawnBody(Vector2 position)
  {
    var body = new MultiShapeBody();
    body.GlobalPosition = position;
    AddChild(body);
  }

  public override void _Input(InputEvent @event)
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
