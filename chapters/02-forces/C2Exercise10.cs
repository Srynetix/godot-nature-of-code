using System.Linq;

using Godot;

public class C2Exercise10 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.10:\n"
      + "Repulsion";
  }

  public class Repulsor : SimpleAttractor
  {
    public override Vector2 Attract(SimpleMover mover)
    {
      return -base.Attract(mover);
    }
  }

  public class MouseAttractor : SimpleAttractor
  {
    private bool active = true;

    public override Vector2 Attract(SimpleMover mover)
    {
      if (!active)
      {
        return Vector2.Zero;
      }

      var mousePos = GetGlobalMousePosition();
      var mouseGravitation = 1;
      var mouseMass = 15;

      var force = mousePos - mover.GlobalPosition;
      var length = Mathf.Clamp(force.Length(), 5, 25);
      float strength = (mouseGravitation * mouseMass * mover.Mass) / (length * length);
      return force.Normalized() * strength;
    }

    public override void _Notification(int what)
    {
      if (what == NotificationWmMouseEnter)
      {
        active = true;
      }
      else if (what == NotificationWmMouseExit)
      {
        active = false;
      }
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new SimpleMover(SimpleMover.WrapModeEnum.Bounce);
      var bodySize = (float)GD.RandRange(20, 40);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);
      var yPos = (float)GD.RandRange(bodySize, size.y - bodySize);
      mover.BodySize = new Vector2(bodySize, bodySize);
      mover.Mass = bodySize;
      mover.Position = new Vector2(xPos, yPos);

      var repulsor = new Repulsor();
      repulsor.Drawing = false;
      mover.AddChild(repulsor);

      var mouseAttractor = new MouseAttractor();
      mouseAttractor.Drawing = false;
      AddChild(mouseAttractor);

      AddChild(mover);
    }
  }
}
