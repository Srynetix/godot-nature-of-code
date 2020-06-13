using Godot;

public class Utils {
    static public float Map(float value, float istart, float istop, float ostart, float ostop) {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    public class Canvas: Node2D {
        private Image image;
        private ImageTexture texture;
        private Sprite sprite;

        public override void _Ready() {
            var size = GetViewport().Size;

            image = new Image();
            image.Create((int)size.x, (int)size.y, false, Image.Format.Rgba8);

            texture = new ImageTexture();
            texture.CreateFromImage(image);

            sprite = new Sprite();
            sprite.Texture = texture;
            sprite.Position = size / 2;

            AddChild(sprite);
        }

        public void Lock() {
            image.Lock();
        }

        public void SetPixel(int x, int y, Color color) {
            image.SetPixel(x, y, color);
        }

        public void UpdateImage() {
            texture.SetData(image);
        }

        public void Unlock() {
            image.Unlock();
        }
    }
}