using Godot;

namespace Automata
{
    public class ColorCell : Cell<Color>
    {
        public override Color GetAliveValue()
        {
            return Colors.White;
        }

        public override Color GetDeadValue()
        {
            return Colors.Black;
        }

        public override void _Draw()
        {
            var color = State;
            DrawRect(new Rect2(Vector2.Zero, Size), color);
        }
    }

    public class ImageProcessingGameOfLife : CellularAutomata2D<ColorCell, Color>
    {
        public Texture SourceTexture { get; set; }
        public float TextureScale { get; set; } = 1;

        public ImageProcessingGameOfLife()
        {
            CenterAlignedGrid = true;
        }

        protected override Vector2 GetAutomataBounds()
        {
            return SourceTexture.GetSize() * TextureScale;
        }

        protected override void InitializeGrid()
        {
            if (SourceTexture == null)
            {
                GD.PrintErr("SourceTexture should have a value.");
                return;
            }

            base.InitializeGrid();
        }

        protected override void InitializeCell(ColorCell cell, int i, int j)
        {
            cell.State = ExtractColorFromImage(i, j);
        }

        // Not needed in this case
        public override void RandomizeGrid() { }

        private Color ExtractColorFromImage(int x, int y)
        {
            var res = (int)(_scale / TextureScale);
            var image = SourceTexture.GetData();
            image.Lock();
            var color = image.GetPixel((x * res) + (res / 2), (y * res) + (res / 2));
            image.Unlock();
            return color;
        }
    }
}
