using Godot;

namespace Physics
{
  public class SimpleTouchSpawner : Node2D
  {
    public delegate Node2D SpawnFunction(Vector2 position);

    public SpawnFunction Spawner = null;
    public Node Container = null;

    public void SpawnBody(Vector2 position)
    {
      if (Spawner != null)
      {
        var body = Spawner(position);

        if (Container == null)
        {
          GetParent().AddChild(body);
        }
        else
        {
          Container.AddChild(body);
        }
      }
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
}
