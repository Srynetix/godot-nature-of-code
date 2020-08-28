using Godot;

public class C5Exercise2 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 5.2:\n"
      + "Anti-Gravity Boxes\n\n"
      + "Touch screen to spawn boxes";
  }

  public class AntigravityBox : Physics.SimpleBox
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

    var spawner = new Physics.SimpleTouchSpawner();
    spawner.Spawner = (position) =>
    {
      var box = new AntigravityBox();
      box.MeshSize = new Vector2(20, 20);
      box.GlobalPosition = position;
      return box;
    };
    AddChild(spawner);

    int boxCount = 10;
    for (int i = 0; i < boxCount; ++i)
    {
      spawner.SpawnBody(MathUtils.RandVector2(0, size.x, 0, size.y));
    }
  }
}
