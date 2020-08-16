using Godot;

public class SimpleSquareParticle : SimpleParticle
{
  public override void _Draw()
  {
    if (IsDead())
    {
      return;
    }

    DrawRect(new Rect2(-BodySize / 2, BodySize / 2), Colors.LightBlue.WithAlpha(GetLifespanAlphaValue()));
  }
}