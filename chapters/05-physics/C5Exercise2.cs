using Godot;

public class C5Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.2:\n"
      + "Box Physics";
  }

  public class Box : RigidBody2D
  {
    public float OutlineWidth = 2;
    public Color OutlineColor = Colors.LightBlue;
    public Color BaseColor = Colors.White;
    public Vector2 BodySize
    {
      get => bodySize;
      set
      {
        bodySize = value;
        if (rectangleShape2D != null)
        {
          rectangleShape2D.Extents = bodySize;
        }
      }
    }

    private Vector2 bodySize = new Vector2(20, 20);
    private CollisionShape2D collisionShape2D;
    private RectangleShape2D rectangleShape2D;

    public override void _Ready()
    {
      rectangleShape2D = new RectangleShape2D();
      rectangleShape2D.Extents = bodySize / 2;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = rectangleShape2D;
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      var outlineVec = new Vector2(OutlineWidth, OutlineWidth);
      DrawRect(new Rect2(-bodySize / 2, bodySize), OutlineColor);
      DrawRect(new Rect2(-bodySize / 2 + outlineVec / 2, bodySize - outlineVec / 2), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }

  public class Wall : StaticBody2D
  {
    public Color BaseColor = Colors.Olive;
    public Vector2 BodySize
    {
      get => bodySize;
      set
      {
        bodySize = value;
        if (rectangleShape2D != null)
        {
          rectangleShape2D.Extents = bodySize;
        }
      }
    }

    private Vector2 bodySize = new Vector2(10, 10);
    private CollisionShape2D collisionShape2D;
    private RectangleShape2D rectangleShape2D;

    public override void _Ready()
    {
      rectangleShape2D = new RectangleShape2D();
      rectangleShape2D.Extents = bodySize / 2;
      collisionShape2D = new CollisionShape2D();
      collisionShape2D.Shape = rectangleShape2D;
      AddChild(collisionShape2D);
    }

    public override void _Draw()
    {
      DrawRect(new Rect2(-bodySize / 2, bodySize), BaseColor);
    }

    public override void _Process(float delta)
    {
      Update();
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;
    var floorHeight = 50;
    var wallWidth = 50;

    // Add floor
    var floor = new Wall();
    floor.BodySize = new Vector2(size.x, floorHeight);
    floor.Position = new Vector2(size.x / 2, size.y);
    AddChild(floor);

    // Add ceiling
    var ceiling = new Wall();
    ceiling.BodySize = new Vector2(size.x, floorHeight);
    ceiling.Position = new Vector2(size.x / 2, 0);
    AddChild(ceiling);

    // Add left wall
    var leftWall = new Wall();
    leftWall.BodySize = new Vector2(wallWidth, size.y);
    leftWall.Position = new Vector2(0, size.y / 2);
    AddChild(leftWall);

    // Add right wall
    var rightWall = new Wall();
    rightWall.BodySize = new Vector2(wallWidth, size.y);
    rightWall.Position = new Vector2(size.x, size.y / 2);
    AddChild(rightWall);

    int boxCount = 10;
    for (int i = 0; i < boxCount; ++i)
    {
      SpawnBox(Utils.RandVector2(wallWidth * 2, size.x - wallWidth * 2, floorHeight * 2, size.y - floorHeight * 2));
    }
  }

  private void SpawnBox(Vector2 position)
  {
    var box = new Box();
    box.BodySize = new Vector2(20, 20);
    box.GlobalPosition = position;
    AddChild(box);
  }

  public override void _Process(float delta)
  {
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventScreenTouch eventScreenTouch)
    {
      if (eventScreenTouch.Pressed)
      {
        SpawnBox(eventScreenTouch.Position);
      }
    }

    if (@event is InputEventScreenDrag eventScreenDrag)
    {
      SpawnBox(eventScreenDrag.Position);
    }
  }
}
