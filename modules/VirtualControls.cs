using Godot;

// Adapted from https://github.com/MarcoFazioRandom/Virtual-Joystick-Godot/blob/master/Joystick/Joystick.gd

public class VirtualControls : Control
{
  public enum JoystickModeEnum
  {
    Fixed,
    Dynamic,
    Following
  }

  public enum VectorModeEnum
  {
    Real,
    Normalized
  }

  public enum VisibilityModeEnum
  {
    Always,
    TouchscreenOnly
  }

  [Export] public float MarginAmount = 8;
  [Export] public bool DebugDraw = false;
  [Export] public JoystickModeEnum JoystickMode = JoystickModeEnum.Fixed;
  [Export] public VectorModeEnum VectorMode = VectorModeEnum.Real;
  [Export] public VisibilityModeEnum VisibilityMode = VisibilityModeEnum.TouchscreenOnly;
  [Export] public Color JoystickPressedColor = Colors.Gray;
  [Export(PropertyHint.Range, "0.5,0.75,0.05")] public float JoystickAnchorTop = 0.55f;
  [Export(PropertyHint.Range, "0,0.75,0.05")] public float JoystickAnchorRight = 0.25f;
  [Export(PropertyHint.Range, "0,12,2")] public int JoystickDirections = 0;
  [Export(PropertyHint.Range, "-180,180")] public float JoystickSymmetryAngle = 90.0f;
  [Export(PropertyHint.Range, "0,0.5")] public float JoystickDeadZone = 0.2f;
  [Export(PropertyHint.Range, "0.5,2")] public float JoystickClampZone = 1;

  // Output values
  public Vector2 JoystickOutput = Vector2.Zero;
  public bool ButtonAPressed = false;
  public bool ButtonBPressed = false;
  public bool JoystickReceivingInputs = false;

  // Private
  private MarginContainer joystickMargin;
  private MarginContainer buttonsMargin;

  public class Joystick : CenterContainer
  {
    public class Background : CenterContainer
    {
      public Color CircleColor = Colors.White.WithAlpha(64);
      public float LineWidth = 2;
      public Color MainLinesColor = Colors.LightBlue.WithAlpha(200);
      public Color SubLinesColor = Colors.LightPink.WithAlpha(150);
      public float Radius => RectSize.x;
      public Vector2 OriginalPosition;

      private Joystick parent;

      public override void _Ready()
      {
        parent = (Joystick)GetParent();
        RectMinSize = new Vector2(96, 96);
      }

      public override void _Draw()
      {
        var center = RectSize / 2;
        var directions = parent.parent.JoystickDirections;
        var symmetryAngle = parent.parent.JoystickSymmetryAngle;
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
          if (directions % 2 == 0 && directions > 4)
          {
            if (i % 2 == 1)
            {
              color = SubLinesColor;
            }
          }

          DrawLine(center, center + (Vector2.Right * Radius).Rotated(-Mathf.Deg2Rad(symmetryAngle + angleAmount * i)), color, LineWidth);
        }
      }

      public override void _Process(float delta)
      {
        Update();
      }
    }

    public class Handle : Control
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

    private VirtualControls parent;
    private Background background;
    private Handle handle;
    private int touchIndex = -1;
    private Font defaultFont;

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
      angle = angle >= 0 ? Mathf.Floor(angle) : Mathf.Ceil(angle);

      if ((int)Mathf.Abs(angle) % 2 == 1)
      {
        angle = angle >= 0 ? angle + 1 : angle - 1;
      }

      angle *= Mathf.Pi / directions;
      angle -= symmetry_angle;
      return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * vector.Length();
    }

    private void Following(Vector2 vector)
    {
      var clampSize = parent.JoystickClampZone * background.Radius;
      if (vector.Length() > clampSize)
      {
        var radius = vector.Normalized() * clampSize;
        var delta = vector - radius;
        var newPos = background.RectPosition + delta;
        newPos.x = Mathf.Clamp(newPos.x, -background.Radius, RectSize.x - background.Radius);
        newPos.y = Mathf.Clamp(newPos.y, -background.Radius, RectSize.y - background.Radius);
        background.RectPosition = newPos;
      }
    }

    private void ResetHandle()
    {
      handle.RectGlobalPosition = background.RectGlobalPosition + background.RectSize / 2 - handle.RectSize / 2;
    }

    private void ResetJoystick()
    {
      // Reset parent values
      parent.JoystickReceivingInputs = false;
      parent.JoystickOutput = Vector2.Zero;

      touchIndex = -1;
      handle.SelfModulate = handle.OriginalColor;
      background.RectPosition = background.OriginalPosition;
      ResetHandle();
    }

    private bool TouchStarted(InputEventScreenTouch eventScreenTouch)
    {
      return eventScreenTouch.Pressed && touchIndex == -1;
    }

    private bool TouchEnded(InputEventScreenTouch eventScreenTouch)
    {
      return !eventScreenTouch.Pressed && touchIndex == eventScreenTouch.Index;
    }

    async public override void _Ready()
    {
      // Parent is a MarginContainer, next parent is VirtualControls
      parent = (VirtualControls)GetParent().GetParent();
      defaultFont = SimpleDefaultFont.LoadDefaultFont();

      // Create background
      background = new Background();
      AddChild(background);

      // Create handle
      handle = new Handle();
      background.AddChild(handle);

      // Check for touchscreen mode
      if (parent.VisibilityMode == VisibilityModeEnum.TouchscreenOnly && !OS.HasTouchscreenUiHint())
      {
        Hide();
      }

      // Wait for next frame to store original position
      await ToSignal(GetTree(), "idle_frame");
      background.OriginalPosition = background.RectPosition;
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
          if (parent.JoystickMode == JoystickModeEnum.Dynamic || parent.JoystickMode == JoystickModeEnum.Following)
          {
            ResetHandle();
          }

          if (PositionIsInRadius(eventScreenTouch.Position, background.RectGlobalPosition + background.RectSize / 2, background.Radius))
          {
            touchIndex = eventScreenTouch.Index;
            handle.SelfModulate = parent.JoystickPressedColor;
          }
        }

        else if (TouchEnded(eventScreenTouch))
        {
          ResetJoystick();
        }
      }

      else if (@event is InputEventScreenDrag eventScreenDrag)
      {
        if (eventScreenDrag.Index != touchIndex)
        {
          return;
        }

        float ray = background.Radius;
        float deadSize = parent.JoystickDeadZone * ray;
        float clampSize = parent.JoystickClampZone * ray;

        var center = background.RectGlobalPosition + background.RectSize / 2;
        var vector = eventScreenDrag.Position - center;

        if (vector.Length() > deadSize)
        {
          if (parent.JoystickDirections > 0)
          {
            vector = DirectionalVector(vector, parent.JoystickDirections, Mathf.Deg2Rad(parent.JoystickSymmetryAngle));
          }

          if (parent.VectorMode == VectorModeEnum.Normalized)
          {
            parent.JoystickOutput = vector.Normalized();
            handle.RectGlobalPosition = parent.JoystickOutput * clampSize + center - handle.RectSize / 2;
          }

          else if (parent.VectorMode == VectorModeEnum.Real)
          {
            var clampedVector = vector.Clamped(clampSize);
            parent.JoystickOutput = vector.Normalized() * (clampedVector.Length() - deadSize) / (clampSize - deadSize);
            handle.RectGlobalPosition = clampedVector + center - handle.RectSize / 2;
          }

          parent.JoystickReceivingInputs = true;
          if (parent.JoystickMode == JoystickModeEnum.Following)
          {
            Following(vector);
          }
        }

        else
        {
          parent.JoystickReceivingInputs = false;
          parent.JoystickOutput = Vector2.Zero;
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
      if (parent.DebugDraw)
      {
        DrawRect(new Rect2(0, 0, RectSize.x, RectSize.y), Colors.LightCyan.WithAlpha(32));
        DrawString(defaultFont, new Vector2(0, -8), "Output: " + parent.JoystickOutput.ToString());
        DrawString(defaultFont, new Vector2(0, -24), "RectPosition: " + background.RectPosition.ToString());
        DrawString(defaultFont, new Vector2(0, -40), "OriginalPosition: " + background.OriginalPosition.ToString());
      }
    }
  }

  public class Buttons : HBoxContainer
  {
    public class TouchButton : Control
    {
      public Color ButtonColor;
      public string Label;
      public bool Pressed;

      private int touchIndex = -1;
      private Buttons parent;
      private Color originalColor;

      public float Radius => RectSize.x;

      public TouchButton(string label, Color color)
      {
        ButtonColor = color;
        Label = label;

        originalColor = color;
      }

      public override void _Ready()
      {
        parent = (Buttons)GetParent();

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
          if (eventScreenTouch.Pressed && eventScreenTouch.Index != touchIndex && eventScreenTouch.Position.DistanceTo(RectGlobalPosition + RectSize / 2) < Radius / 2)
          {
            ButtonColor = Colors.Yellow;
            Pressed = true;
            touchIndex = eventScreenTouch.Index;
          }

          if (!eventScreenTouch.Pressed && eventScreenTouch.Index == touchIndex)
          {
            ButtonColor = originalColor;
            Pressed = false;
            touchIndex = -1;
          }
        }
      }

      public override void _Draw()
      {
        var labelSize = parent.defaultFont.GetStringSize(Label);
        DrawCircle(RectSize / 2, RectSize.x / 2, ButtonColor);
        DrawString(parent.defaultFont, RectSize / 2 - new Vector2(labelSize.x / 2, -labelSize.y / 4), Label);
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

    private TouchButton buttonA;
    private TouchButton buttonB;
    private VirtualControls parent;
    private Font defaultFont;

    public override void _Ready()
    {
      // Parent is a MarginContainer, next parent is VirtualControls
      parent = (VirtualControls)GetParent().GetParent();
      defaultFont = SimpleDefaultFont.LoadDefaultFont();

      // Add buttons
      buttonA = new TouchButton(ButtonALabel, ButtonAColor);
      AddChild(buttonA);
      buttonB = new TouchButton(ButtonBLabel, ButtonBColor);
      AddChild(buttonB);

      // Check for touchscreen mode
      if (parent.VisibilityMode == VisibilityModeEnum.TouchscreenOnly && !OS.HasTouchscreenUiHint())
      {
        Hide();
      }
    }

    public override void _Draw()
    {
      if (parent.DebugDraw)
      {
        DrawRect(new Rect2(0, 0, RectSize.x, RectSize.y), Colors.LightCyan.WithAlpha(32));
        DrawString(defaultFont, new Vector2(0, -8), "A: " + buttonA.Pressed.ToString() + "  -  " + "B: " + buttonB.Pressed.ToString());
      }
    }

    public override void _Process(float delta)
    {
      parent.ButtonAPressed = buttonA.Pressed;
      parent.ButtonBPressed = buttonB.Pressed;

      Update();
    }
  }

  private void _UpdateMargins()
  {
    joystickMargin.AnchorTop = JoystickAnchorTop;
    joystickMargin.AnchorRight = JoystickAnchorRight;
    joystickMargin.AnchorBottom = 1.0f;
    joystickMargin.Set("custom_constants/margin_left", MarginAmount);
    joystickMargin.Set("custom_constants/margin_right", MarginAmount);
    joystickMargin.Set("custom_constants/margin_top", MarginAmount);
    joystickMargin.Set("custom_constants/margin_bottom", MarginAmount);

    buttonsMargin.AnchorTop = 0.75f;
    buttonsMargin.AnchorRight = 1.0f;
    buttonsMargin.AnchorLeft = 0.75f;
    buttonsMargin.AnchorBottom = 1.0f;
    buttonsMargin.Set("custom_constants/margin_left", MarginAmount);
    buttonsMargin.Set("custom_constants/margin_right", MarginAmount);
    buttonsMargin.Set("custom_constants/margin_top", MarginAmount);
    buttonsMargin.Set("custom_constants/margin_bottom", MarginAmount);
  }

  public override void _Process(float delta)
  {
    if (Engine.EditorHint)
    {
      _UpdateMargins();
    }
  }

  public override void _Ready()
  {
    AnchorRight = 1.0f;
    AnchorBottom = 1.0f;

    var viewportSize = GetViewportRect().Size;

    // Create joystick
    joystickMargin = new MarginContainer();
    AddChild(joystickMargin);
    var joystick = new Joystick();
    joystickMargin.AddChild(joystick);

    // Create buttons
    buttonsMargin = new MarginContainer();
    AddChild(buttonsMargin);
    var buttons = new Buttons();
    buttonsMargin.AddChild(buttons);

    _UpdateMargins();
  }
}
