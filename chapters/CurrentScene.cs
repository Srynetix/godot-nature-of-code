using Godot;

public class CurrentScene : MarginContainer
{
  public override void _UnhandledInput(InputEvent @event)
  {
    GD.Print("CurrentScene _UnhandledInput", @event);
  }
}
