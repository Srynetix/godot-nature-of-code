using Godot;

public class SimpleLiquid : SimpleZone
{
  public float Coeff = 0.25f;

  public override void _Draw()
  {
    DrawZone(Colors.DarkViolet);

    var strToDraw = "Liquid: " + Coeff.ToString();
    var strSize = defaultFont.GetStringSize(strToDraw);
    DrawString(defaultFont, Vector2.Left * strSize / 2, strToDraw);
  }

  public override void _Process(float delta)
  {
    foreach (var area in GetOverlappingAreas())
    {
      var mover = (SimpleMover)area;
      mover.ApplyDrag(Coeff);
    }
  }
}
