using Godot;

namespace Physics
{
  /// <summary>
  /// Touch spawner.
  /// Touch anywhere and spawn something.
  /// Can also spawn on demand using `SpawnBody`.
  /// </summary>
  public class SimpleTouchSpawner : Node2D
  {
    /// <summary>Spawn function definition</summary>
    public delegate Node2D SpawnFuncDef(Vector2 position);

    /// <summary>Spawn function</summary>
    public SpawnFuncDef SpawnFunction = null;
    /// <summary>Target container. Defaults to parent.
    public Node Container = null;

    /// <summary>
    /// Spawn body at position.
    /// </summary>
    /// <param name="position">Target position</param>
    public void SpawnBody(Vector2 position)
    {
      if (SpawnFunction != null)
      {
        var body = SpawnFunction(position);
        var container = Container ?? GetParent();
        container.AddChild(body);
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
