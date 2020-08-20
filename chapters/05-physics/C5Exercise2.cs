using Godot;

public class C5Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.2:\n"
      + "Anti-Gravity Boxes\n\n"
      + "Touch screen to spawn boxes";
  }

  public class AntigravityBox : SimpleBox
  {
    public override void _Ready()
    {
      base._Ready();

      // Remove gravity
      GravityScale = 0;
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    int boxCount = 10;
    for (int i = 0; i < boxCount; ++i)
    {
      SpawnBox(Utils.RandVector2(0, size.x, 0, size.y));
    }
  }

  private void SpawnBox(Vector2 position)
  {
    var box = new AntigravityBox();
    box.BodySize = new Vector2(20, 20);
    box.GlobalPosition = position;
    AddChild(box);
  }

  public override void _UnhandledInput(InputEvent @event)
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
