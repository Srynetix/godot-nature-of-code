using System.Linq;
using Godot;
using Drawing;
using Forces;

public class C2Exercise6 : Node2D, IExample
{
  public string _Summary()
  {
    return "Exercise 2.6:\n"
      + "Water Drag Surface";
  }

  public class Mover : SimpleMover
  {
    public Mover() : base(WrapModeEnum.Bounce)
    {
      Mesh.MeshType = SimpleMesh.TypeEnum.Square;
    }

    public override void ApplyDrag(float coef)
    {
      float speedSqr = Velocity.LengthSquared();
      float mag = coef * speedSqr;

      var drag = Velocity.Normalized() * mag * -1;
      // Add surface area = MeshSize
      drag *= MeshSize / 10;

      ApplyForce(drag);
    }

    protected override void UpdateAcceleration()
    {
      var wind = new Vector2(0.1f, 0);
      var gravity = new Vector2(0, 0.098f * Mass);

      ApplyForce(wind);
      ApplyForce(gravity);
    }
  }

  public override void _Ready()
  {
    var size = GetViewportRect().Size;

    var zone = new SimpleLiquid();
    zone.Coeff = 0.25f;
    zone.Size = new Vector2(size.x, size.y / 4);
    zone.Position = new Vector2(size.x / 2, size.y - size.y / 8);
    AddChild(zone);

    foreach (var x in Enumerable.Range(0, 20))
    {
      var mover = new Mover();
      var bodySize = (float)GD.RandRange(10, 40);
      var xPos = (float)GD.RandRange(bodySize, size.x - bodySize);

      mover.MeshSize = new Vector2(bodySize, bodySize);
      mover.Mass = (float)GD.RandRange(5, 10);
      mover.Position = new Vector2(xPos, size.y / 2 + (float)GD.RandRange(-100, 100));

      AddChild(mover);
    }
  }
}
