using Godot;
using Assets;

/// <summary>
/// Embedded virtual controls.
/// Adapted from https://github.com/MarcoFazioRandom/Virtual-Joystick-Godot
/// </summary>
public class VirtualControls : Control
{
    /// <summary>
    /// Joystick mode.
    /// </summary>
    public enum JoystickModeEnum
    {
        /// <summary>Fixed joystick</summary>
        Fixed,

        /// <summary>Dynamic joystick</summary>
        Dynamic,

        /// <summary>Following joystick (follow drag movement)</summary>
        Following
    }

    /// <summary>
    /// Vector normalization mode.
    /// </summary>
    public enum VectorModeEnum
    {
        /// <summary>Real vector value</summary>
        Real,

        /// <summary>Normalized vector value</summary>
        Normalized
    }

    /// <summary>
    /// Visibility mode.
    /// </summary>
    public enum VisibilityModeEnum
    {
        /// <summary>Always show joystick</summary>
        Always,

        /// <summary>Only show on touchscreen devices</summary>
        TouchscreenOnly
    }

    /// <summary>External margin amount</summary>
    [Export] public float MarginAmount = 8;

    /// <summary>Debug draw information</summary>
    [Export] public bool DebugDraw;

    /// <summary>Joystick mode</summary>
    [Export] public JoystickModeEnum JoystickMode;

    /// <summary>Vector mode</summary>
    [Export] public VectorModeEnum VectorMode;

    /// <summary>Visibility mode</summary>
    [Export] public VisibilityModeEnum VisibilityMode = VisibilityModeEnum.TouchscreenOnly;

    /// <summary>Joystick pressed color</summary>
    [Export] public Color JoystickPressedColor = Colors.Gray;

    /// <summary>Joystick anchor top</summary>
    [Export(PropertyHint.Range, "0.5,0.75,0.05")] public float JoystickAnchorTop = 0.55f;

    /// <summary>Joystick anchor right</summary>
    [Export(PropertyHint.Range, "0,0.75,0.05")] public float JoystickAnchorRight = 0.25f;

    /// <summary>Joystick fixed directions (between 0 (free) and 12)</summary>
    [Export(PropertyHint.Range, "0,12,2")] public int JoystickDirections;

    /// <summary>Joystick symmetry angle</summary>
    [Export(PropertyHint.Range, "-180,180")] public float JoystickSymmetryAngle = 90.0f;

    /// <summary>Joystick dead zone</summary>
    [Export(PropertyHint.Range, "0,0.5")] public float JoystickDeadZone = 0.2f;

    /// <summary>Joystick clamp zone</summary>
    [Export(PropertyHint.Range, "0.5,2")] public float JoystickClampZone = 1;

    /// <summary>Joystick output</summary>
    public Vector2 JoystickOutput = Vector2.Zero;

    /// <summary>Button A is currently pressed</summary>
    public bool ButtonAPressed;

    /// <summary>Button B is currently pressed</summary>
    public bool ButtonBPressed;

    /// <summary>Joystick is receiving inputs</summary>
    public bool JoystickReceivingInputs;

    private MarginContainer _joystickMargin;
    private MarginContainer _buttonsMargin;

    private class Joystick : CenterContainer
    {
        private class Background : CenterContainer
        {
            public Color CircleColor = Colors.White.WithAlpha(64);
            public float LineWidth = 2;
            public Color MainLinesColor = Colors.LightBlue.WithAlpha(200);
            public Color SubLinesColor = Colors.LightPink.WithAlpha(150);
            public float Radius => RectSize.x;
            public Vector2 OriginalPosition;

            private Joystick _parent;

            public override void _Ready()
            {
                _parent = (Joystick)GetParent();
                RectMinSize = new Vector2(96, 96);
            }

            public override void _Draw()
            {
                var center = RectSize / 2;
                var directions = _parent._parent.JoystickDirections;
                var symmetryAngle = _parent._parent.JoystickSymmetryAngle;
                DrawCircle(center, Radius, CircleColor);

                if (directions == 0)
                {
                    // No lines
                    return;
                }

                // Draw lines
                var angleAmount = 360 / directions;
                for (int i = 0; i < directions; i++)
                {
                    var color = MainLinesColor;

                    // Handle sub lines
                    if (directions % 2 == 0 && directions > 4 && i % 2 == 1)
                    {
                        color = SubLinesColor;
                    }

                    DrawLine(center, center + (Vector2.Right * Radius).Rotated(-Mathf.Deg2Rad(symmetryAngle + (angleAmount * i))), color, LineWidth);
                }
            }

            public override void _Process(float delta)
            {
                Update();
            }
        }

        private class Handle : Control
        {
            public Color OriginalColor;
            public float Radius => RectSize.x;

            public override void _Ready()
            {
                RectMinSize = new Vector2(8, 8);
                OriginalColor = SelfModulate;
            }

            public override void _Draw()
            {
                DrawCircle(RectSize / 2, Radius, Colors.LightBlue.WithAlpha(128));
            }
        }

        private VirtualControls _parent;
        private Background _background;
        private Handle _handle;
        private int _touchIndex = -1;
        private Font _defaultFont;

        public override async void _Ready()
        {
            // Parent is a MarginContainer, next parent is VirtualControls
            _parent = (VirtualControls)GetParent().GetParent();
            _defaultFont = SimpleDefaultFont.Regular;

            // Create background
            _background = new Background();
            AddChild(_background);

            // Create handle
            _handle = new Handle();
            _background.AddChild(_handle);

            // Check for touchscreen mode
            if (_parent.VisibilityMode == VisibilityModeEnum.TouchscreenOnly && !OS.HasTouchscreenUiHint())
            {
                Hide();
            }

            // Wait for next frame to store original position
            await ToSignal(GetTree(), "idle_frame");
            _background.OriginalPosition = _background.RectPosition;
        }

        public override void _Input(InputEvent @event)
        {
            if (!(@event is InputEventScreenTouch) && !(@event is InputEventScreenDrag))
            {
                return;
            }

            if (@event is InputEventScreenTouch eventScreenTouch)
            {
                if (TouchStarted(eventScreenTouch) && PositionIsInRect(eventScreenTouch.Position, RectGlobalPosition, RectSize))
                {
                    if (_parent.JoystickMode == JoystickModeEnum.Dynamic || _parent.JoystickMode == JoystickModeEnum.Following)
                    {
                        ResetHandle();
                    }

                    if (PositionIsInRadius(eventScreenTouch.Position, _background.RectGlobalPosition + (_background.RectSize / 2), _background.Radius))
                    {
                        _touchIndex = eventScreenTouch.Index;
                        _handle.SelfModulate = _parent.JoystickPressedColor;
                    }
                }
                else if (TouchEnded(eventScreenTouch))
                {
                    ResetJoystick();
                }
            }
            else if (@event is InputEventScreenDrag eventScreenDrag)
            {
                if (eventScreenDrag.Index != _touchIndex)
                {
                    return;
                }

                float ray = _background.Radius;
                float deadSize = _parent.JoystickDeadZone * ray;
                float clampSize = _parent.JoystickClampZone * ray;

                var center = _background.RectGlobalPosition + (_background.RectSize / 2);
                var vector = eventScreenDrag.Position - center;

                if (vector.Length() > deadSize)
                {
                    if (_parent.JoystickDirections > 0)
                    {
                        vector = DirectionalVector(vector, _parent.JoystickDirections, Mathf.Deg2Rad(_parent.JoystickSymmetryAngle));
                    }

                    if (_parent.VectorMode == VectorModeEnum.Normalized)
                    {
                        _parent.JoystickOutput = vector.Normalized();
                        _handle.RectGlobalPosition = (_parent.JoystickOutput * clampSize) + center - (_handle.RectSize / 2);
                    }
                    else if (_parent.VectorMode == VectorModeEnum.Real)
                    {
                        var clampedVector = vector.Clamped(clampSize);
                        _parent.JoystickOutput = vector.Normalized() * (clampedVector.Length() - deadSize) / (clampSize - deadSize);
                        _handle.RectGlobalPosition = clampedVector + center - (_handle.RectSize / 2);
                    }

                    _parent.JoystickReceivingInputs = true;
                    if (_parent.JoystickMode == JoystickModeEnum.Following)
                    {
                        Following(vector);
                    }
                }
                else
                {
                    _parent.JoystickReceivingInputs = false;
                    _parent.JoystickOutput = Vector2.Zero;
                    ResetHandle();
                    return;
                }
            }
        }

        public override void _Process(float delta)
        {
            Update();
        }

        public override void _Draw()
        {
            if (_parent.DebugDraw)
            {
                DrawRect(new Rect2(0, 0, RectSize.x, RectSize.y), Colors.LightCyan.WithAlpha(32));
                DrawString(_defaultFont, new Vector2(0, -8), "Output: " + _parent.JoystickOutput.ToString());
                DrawString(_defaultFont, new Vector2(0, -24), "RectPosition: " + _background.RectPosition.ToString());
                DrawString(_defaultFont, new Vector2(0, -40), "OriginalPosition: " + _background.OriginalPosition.ToString());
            }
        }

        private bool PositionIsInRadius(Vector2 sourcePosition, Vector2 targetPosition, float targetRadius)
        {
            return sourcePosition.DistanceTo(targetPosition) < targetRadius;
        }

        private bool PositionIsInRect(Vector2 sourcePosition, Vector2 targetPosition, Vector2 targetSize)
        {
            return new Rect2(targetPosition, targetSize).HasPoint(sourcePosition);
        }

        private Vector2 DirectionalVector(Vector2 vector, int directions, float symmetry_angle = Mathf.Pi / 2.0f)
        {
            var angle = (vector.Angle() + symmetry_angle) / (Mathf.Pi / directions);
            angle = (angle >= 0) ? Mathf.Floor(angle) : Mathf.Ceil(angle);

            if ((int)Mathf.Abs(angle) % 2 == 1)
            {
                angle = (angle >= 0) ? angle + 1 : angle - 1;
            }

            angle *= Mathf.Pi / directions;
            angle -= symmetry_angle;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * vector.Length();
        }

        private void Following(Vector2 vector)
        {
            var clampSize = _parent.JoystickClampZone * _background.Radius;
            if (vector.Length() > clampSize)
            {
                var radius = vector.Normalized() * clampSize;
                var delta = vector - radius;
                var newPos = _background.RectPosition + delta;
                newPos.x = Mathf.Clamp(newPos.x, -_background.Radius, RectSize.x - _background.Radius);
                newPos.y = Mathf.Clamp(newPos.y, -_background.Radius, RectSize.y - _background.Radius);
                _background.RectPosition = newPos;
            }
        }

        private void ResetHandle()
        {
            _handle.RectGlobalPosition = _background.RectGlobalPosition + (_background.RectSize / 2) - (_handle.RectSize / 2);
        }

        private void ResetJoystick()
        {
            // Reset parent values
            _parent.JoystickReceivingInputs = false;
            _parent.JoystickOutput = Vector2.Zero;

            _touchIndex = -1;
            _handle.SelfModulate = _handle.OriginalColor;
            _background.RectPosition = _background.OriginalPosition;
            ResetHandle();
        }

        private bool TouchStarted(InputEventScreenTouch eventScreenTouch)
        {
            return eventScreenTouch.Pressed && _touchIndex == -1;
        }

        private bool TouchEnded(InputEventScreenTouch eventScreenTouch)
        {
            return !eventScreenTouch.Pressed && _touchIndex == eventScreenTouch.Index;
        }
    }

    private class Buttons : HBoxContainer
    {
        private class TouchButton : Control
        {
            public Color ButtonColor;
            public string Label;
            public bool Pressed;

            private int _touchIndex = -1;
            private Buttons _parent;
            private Color _originalColor;

            public float Radius => RectSize.x;

            public TouchButton(string label, Color color)
            {
                ButtonColor = color;
                Label = label;

                _originalColor = color;
            }

            public override void _Ready()
            {
                _parent = (Buttons)GetParent();

                RectMinSize = new Vector2(96, 96);
                SizeFlagsHorizontal = (int)SizeFlags.Expand | (int)SizeFlags.ShrinkCenter;
            }

            public override void _Input(InputEvent @event)
            {
                if (!(@event is InputEventScreenTouch) && !(@event is InputEventScreenDrag))
                {
                    return;
                }

                if (@event is InputEventScreenTouch eventScreenTouch)
                {
                    if (eventScreenTouch.Pressed && eventScreenTouch.Index != _touchIndex && eventScreenTouch.Position.DistanceTo(RectGlobalPosition + (RectSize / 2)) < Radius / 2)
                    {
                        ButtonColor = Colors.Yellow;
                        Pressed = true;
                        _touchIndex = eventScreenTouch.Index;
                    }

                    if (!eventScreenTouch.Pressed && eventScreenTouch.Index == _touchIndex)
                    {
                        ButtonColor = _originalColor;
                        Pressed = false;
                        _touchIndex = -1;
                    }
                }
            }

            public override void _Draw()
            {
                var labelSize = _parent._defaultFont.GetStringSize(Label);
                DrawCircle(RectSize / 2, RectSize.x / 2, ButtonColor);
                DrawString(_parent._defaultFont, (RectSize / 2) - new Vector2(labelSize.x / 2, -labelSize.y / 4), Label);
            }

            public override void _Process(float delta)
            {
                Update();
            }
        }

        public Color PressedColor = Colors.White;
        public string ButtonALabel = "A";
        public Color ButtonAColor = Colors.AliceBlue;
        public string ButtonBLabel = "B";
        public Color ButtonBColor = Colors.Bisque;

        private TouchButton _buttonA;
        private TouchButton _buttonB;
        private VirtualControls _parent;
        private Font _defaultFont;

        public override void _Ready()
        {
            // Parent is a MarginContainer, next parent is VirtualControls
            _parent = (VirtualControls)GetParent().GetParent();
            _defaultFont = SimpleDefaultFont.Regular;

            // Add buttons
            _buttonA = new TouchButton(ButtonALabel, ButtonAColor);
            AddChild(_buttonA);
            _buttonB = new TouchButton(ButtonBLabel, ButtonBColor);
            AddChild(_buttonB);

            // Check for touchscreen mode
            if (_parent.VisibilityMode == VisibilityModeEnum.TouchscreenOnly && !OS.HasTouchscreenUiHint())
            {
                Hide();
            }
        }

        public override void _Draw()
        {
            if (_parent.DebugDraw)
            {
                DrawRect(new Rect2(0, 0, RectSize.x, RectSize.y), Colors.LightCyan.WithAlpha(32));
                DrawString(_defaultFont, new Vector2(0, -8), "A: " + _buttonA.Pressed.ToString() + "  -  B: " + _buttonB.Pressed.ToString());
            }
        }

        public override void _Process(float delta)
        {
            _parent.ButtonAPressed = _buttonA.Pressed;
            _parent.ButtonBPressed = _buttonB.Pressed;

            Update();
        }
    }

    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            UpdateMargins();
        }
    }

    public override void _Ready()
    {
        AnchorRight = 1.0f;
        AnchorBottom = 1.0f;

        // Create joystick
        _joystickMargin = new MarginContainer();
        AddChild(_joystickMargin);
        var joystick = new Joystick();
        _joystickMargin.AddChild(joystick);

        // Create buttons
        _buttonsMargin = new MarginContainer();
        AddChild(_buttonsMargin);
        var buttons = new Buttons();
        _buttonsMargin.AddChild(buttons);

        UpdateMargins();
    }

    private void UpdateMargins()
    {
        _joystickMargin.AnchorTop = JoystickAnchorTop;
        _joystickMargin.AnchorRight = JoystickAnchorRight;
        _joystickMargin.AnchorBottom = 1.0f;
        _joystickMargin.Set("custom_constants/margin_left", MarginAmount);
        _joystickMargin.Set("custom_constants/margin_right", MarginAmount);
        _joystickMargin.Set("custom_constants/margin_top", MarginAmount);
        _joystickMargin.Set("custom_constants/margin_bottom", MarginAmount);

        _buttonsMargin.AnchorTop = 0.75f;
        _buttonsMargin.AnchorRight = 1.0f;
        _buttonsMargin.AnchorLeft = 0.75f;
        _buttonsMargin.AnchorBottom = 1.0f;
        _buttonsMargin.Set("custom_constants/margin_left", MarginAmount);
        _buttonsMargin.Set("custom_constants/margin_right", MarginAmount);
        _buttonsMargin.Set("custom_constants/margin_top", MarginAmount);
        _buttonsMargin.Set("custom_constants/margin_bottom", MarginAmount);
    }
}
