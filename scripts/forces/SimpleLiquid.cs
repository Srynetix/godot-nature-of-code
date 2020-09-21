using Godot;

namespace Forces
{
    /// <summary>
    /// Simple liquid pocket. Apply drag in a delimited zone.
    /// </summary>
    public class SimpleLiquid : SimpleZone
    {
        /// <summary>Drag coefficient</summary>
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
}
