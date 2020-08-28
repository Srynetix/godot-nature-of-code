using Godot;
using Forces;

public class C2Example1 : Node2D, IExample
{
  public string _Summary()
  {
    return "Example 2.1:\n"
      + "Forces";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce) { }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.01f, 0);
      var gravity = new Vector2(0, 0.09f);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    var mover = new Mover();
    mover.Position = GetViewportRect().Size / 2;
    AddChild(mover);
  }
}
