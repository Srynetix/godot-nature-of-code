using Godot;
using System;

public class Utils {
    static public float Map(float value, float istart, float istop, float ostart, float ostop) {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    public class Canvas: Control {
        public delegate void DrawFunction(Node2D pen);
        
        private Viewport viewport;
        private Node2D pen;
        private TextureRect board;
        private DrawFunction drawFunction = null;

        public void SetDrawFunction(DrawFunction fn) {
            drawFunction = fn;
        }

        public override void _Ready() {
            var size = GetViewport().Size;
            viewport = new Viewport();
            viewport.Size = size;
            viewport.Usage = Viewport.UsageEnum.Usage2d;
            viewport.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
            viewport.RenderTargetClearMode = Viewport.ClearMode.OnlyNextFrame;
            viewport.RenderTargetVFlip = true;

            var colorRect = new ColorRect();
            colorRect.Modulate = Colors.Blue;
            viewport.AddChild(colorRect);
            
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
            if (drawFunction != null) {
                drawFunction(pen);
            }
        }

        public override void _Process(float delta) {
            pen.Update();
        }
    }
}