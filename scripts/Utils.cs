using Godot;

public static class ColorExtensions {
    public static Color WithAlpha(this Color color, byte alpha) {
        Color clone = color;
        clone.a8 = alpha;
        return clone;
    }
}

public class Utils {
    /**
     * Map a value from one bound to another.
     */
    static public float Map(float value, float istart, float istop, float ostart, float ostop) {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    /**
     * Return a signed Randf, between -1 and 1.
     */
    static public float SignedRandf() {
        return Map(GD.Randf(), 0, 1, -1, 1);
    }

    public class Canvas: Control {
        public delegate void DrawFunction(Node2D pen);
        
        private Viewport viewport;
        private Node2D pen;
        private TextureRect board;
        private Color backgroundColor = Color.Color8(45, 45, 45);
        private DrawFunction drawFunction = null;

        public void SetDrawFunction(DrawFunction fn) {
            drawFunction = fn;
        }

        public void SetBackgroundColor(Color color) {
            backgroundColor = color;
        }

        public override void _Ready() {
            var size = GetViewport().Size;
            viewport = new Viewport();
            viewport.Size = size;
            viewport.Usage = Viewport.UsageEnum.Usage2d;
            viewport.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
            viewport.RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
            viewport.RenderTargetVFlip = true;

            pen = new Node2D();
            pen.Connect("draw", this, nameof(OnPenDraw));
            viewport.AddChild(pen);
            AddChild(viewport);

            var texture = viewport.GetTexture();
            board = new TextureRect();
            board.Texture = texture;
            AddChild(board);
        }

        public void OnPenDraw() {
            VisualServer.SetDefaultClearColor(backgroundColor);

            if (drawFunction != null) {
                drawFunction(pen);
            }
        }

        public override void _Process(float delta) {
            pen.Update();
        }
    }
}