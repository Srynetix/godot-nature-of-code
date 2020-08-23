using Godot;

public class SimpleFrictionPocket : SimpleZone
{
  public float Coeff = 0;

  public override void _Draw()
  {
    Color color;
    if (Coeff > 0)
    {
      color = Colors.DarkRed;
    }
    else
    {
      color = Colors.LightBlue;
    }

    DrawZone(color);

    var strToDraw = "Friction: " + Coeff.ToString();
    var strSize = defaultFont.GetStringSize(strToDraw);
    DrawString(defaultFont, Vector2.Left * strSize / 2, strToDraw);
  }

  public override void _Process(float delta)
  {
    foreach (var area in GetOverlappingAreas())
    {
      var mover = (SimpleMover)area;
      mover.ApplyFriction(Coeff);
    }
  }
}
