using Godot;
using Assets;

namespace Automata
{
  /// <summary>
  /// Touch behavior enum.
  /// </summary>
  public enum TouchBehaviorEnum
  {
    /// <summary>Randomize grid</summary>
    RandomizeGrid,

    /// <summary>Draw cell at touch position</summary>
    DrawCell
  }

  /// <summary>
  /// Wrap behavior enum.
  ///</summary>
  public enum WrapBehaviorEnum
  {
    /// <summary>Wrap cells</summary>
    Wrap,

    /// <summary>Do not wrap cells</summary>
    Nowrap
  }

  /// <summary>
  /// Cellular automata 2D.
  /// </summary>
  public class CellularAutomata2D : Node2D
  {
    /// <summary>
    /// Cell.
    /// </summary>
    public class Cell : Node2D
    {
      /// <summary>Previous state</summary>
      public int PreviousState { set; get; }

      /// <summary>Current state</summary>
      public int State { set; get; }

      /// <summary>Cell size</summary>
      public Vector2 Size { set; get; }

      /// <summary>Cell color</summary>
      public Color CellColor { set; get; } = Colors.LightBlue;

      /// <summary>Highlight when a cell is transitioning from life to death</summary>
      public bool HighlightTransitions { set; get; }

      /// <summary>
      /// Creates a new cell.
      /// </summary>
      public Cell()
      {
        Name = "Cell";
      }

      public override void _Draw()
      {
        var color = GetStateColor();
        if (color.HasValue)
        {
          DrawRect(new Rect2(Vector2.Zero, Size), color.Value);
        }
      }

      public override void _Process(float delta)
      {
        Update();
      }

      /// <summary>
      /// Get state color.
      /// Returns null if cell is discarded.
      /// </summary>
      /// <returns>State color</returns>
      protected Color? GetStateColor()
      {
        if (HighlightTransitions)
        {
          if (PreviousState == 0 && State == 1)
          {
            // Cell just lived
            return Colors.Blue;
          }
          else if (PreviousState == 1 && State == 0)
          {
            // Cell just died
            return Colors.Red;
          }
          else if (State == 0)
          {
            // Cell already dead
            return null;
          }
        }
        else if (State == 0)
        {
          // Cell already dead
          return null;
        }

        return CellColor;
      }
    }

    /// <summary>Wait time</summary>
    public float WaitTime
    {
      get => _waitTime;
      set
      {
        _waitTime = value;
        if (_timer != null)
        {
          _timer.WaitTime = value;
        }
      }
    }

    /// <summary>Cell color</summary>
    public Color CellColor { get; set; } = Colors.LightBlue;

    /// <summary>Paused</summary>
    public bool Paused { get; set; }

    /// <summary>Highlight when a cell is transitioning from life to death</summary>
    public bool HighlightTransitions { set; get; }

    /// <summary>Touch behavior</summary>
    public TouchBehaviorEnum TouchBehavior { get; set; }

    /// <summary>Wrap behavior</summary>
    public WrapBehaviorEnum WrapBehavior { get; set; }

    /// <summary>Grid nodes</summary>
    protected Cell[] _grid;

    /// <summary>Cell scale</summary>
    protected int _scale;

    /// <summary>Row count</summary>
    protected int _rows;

    /// <summary>Cols count</summary>
    protected int _cols;

    /// <summary>Current generation</summary>
    protected int _generation;

    private Timer _timer;
    private RichTextLabel _label;
    private Button _pauseButton;
    private float _waitTime = 0.05f;
    private int _touchIndex = -1;

    /// <summary>
    /// Create a default cellular automata with a cell scale of 20.
    /// </summary>
    public CellularAutomata2D() : this(20) { }

    /// <summary>
    /// Create a default cellular automata with a custom cell scale.
    /// </summary>
    /// <param name="scale">Scale</param>
    public CellularAutomata2D(int scale)
    {
      Name = "CellularAutomata2D";
      _scale = scale;
    }

    /// <summary>
    /// Randomize grid.
    /// </summary>
    public virtual void RandomizeGrid()
    {
      _generation = 0;
      int offset = WrapBehavior == WrapBehaviorEnum.Wrap ? 0 : 1;

      for (int j = offset; j < _rows - offset; ++j)
      {
        for (int i = offset; i < _cols - offset; ++i)
        {
          var currPos = i + (j * _cols);
          var cell = _grid[currPos];
          cell.State = cell.PreviousState = MathUtils.RandRangei(0, 1);
        }
      }
    }

    public override void _Ready()
    {
      // Create automata on ready
      var size = GetViewportRect().Size;
      InitializeGrid();

      // Create timer
      _timer = new Timer
      {
        Name = "UpdateTimer",
        WaitTime = _waitTime,
        Autostart = true
      };
      AddChild(_timer);
      _timer.Connect("timeout", this, nameof(TryGenerate));

      // Create label
      var font = SimpleDefaultFont.Regular;
      var textSize = font.GetStringSize("Generation: 0000000");
      _label = new RichTextLabel
      {
        Name = "Label",
        BbcodeEnabled = true,
        ScrollActive = false
      };
      _label.RectGlobalPosition = new Vector2(8, (size.y / 2) - textSize.y);
      _label.RectMinSize = new Vector2(textSize.x, textSize.y);
      _label.Set("custom_fonts/normal_font", font);
      AddChild(_label);

      // Create button
      _pauseButton = new Button
      {
        Name = "Pause Button"
      };

      _pauseButton.Set("custom_fonts/font", font);
      _pauseButton.Text = "Touch here to pause";
      _pauseButton.Flat = true;
      _pauseButton.RectMinSize = new Vector2(64, 16);
      _pauseButton.RectGlobalPosition = new Vector2(0, (size.y / 2) - textSize.y + textSize.y);
      AddChild(_pauseButton);
      _pauseButton.Connect("pressed", this, nameof(TogglePause));
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
      if (@event is InputEventScreenTouch eventScreenTouch)
      {
        if (eventScreenTouch.Pressed && _touchIndex == -1)
        {
          _touchIndex = eventScreenTouch.Index;
          if (TouchBehavior == TouchBehaviorEnum.DrawCell)
          {
            ReviveCellAtScreenPos(eventScreenTouch.Position);
          }
          else
          {
            RandomizeGrid();
          }
        }
        else if (!eventScreenTouch.Pressed && _touchIndex == eventScreenTouch.Index)
        {
          _touchIndex = -1;
        }
      }
      else if (@event is InputEventScreenDrag eventScreenDrag)
      {
        if (eventScreenDrag.Index == _touchIndex && TouchBehavior == TouchBehaviorEnum.DrawCell)
        {
          ReviveCellAtScreenPos(eventScreenDrag.Position);
        }
      }
    }

    /// <summary>
    /// Initialize grid.
    /// </summary>
    protected virtual void InitializeGrid()
    {
      var size = GetViewportRect().Size;
      _cols = (int)size.x / _scale;
      _rows = (int)size.y / _scale;
      _grid = new Cell[_cols * _rows];

      for (int j = 0; j < _rows; ++j)
      {
        for (int i = 0; i < _cols; ++i)
        {
          var currPos = i + (j * _cols);
          var cell = new Cell
          {
            Position = new Vector2(i * _scale, j * _scale),
            Size = new Vector2(_scale, _scale),
            PreviousState = 0,
            State = 0,
            CellColor = CellColor,
            HighlightTransitions = HighlightTransitions
          };
          AddChild(cell);
          _grid[currPos] = cell;
        }
      }
    }

    /// <summary>
    /// Revive cell at screen position.
    /// </summary>
    /// <param name="pos">Screen position</param>
    protected virtual void ReviveCellAtScreenPos(Vector2 pos)
    {
      // Split position depending on scale
      var idx = pos / _scale;
      int offset = WrapBehavior == WrapBehaviorEnum.Wrap ? 0 : 1;
      int x = Mathf.Min(Mathf.Max(offset, (int)idx.x), _cols - 1 - offset);
      int y = Mathf.Min(Mathf.Max(offset, (int)idx.y), _rows - 1 - offset);

      var cell = _grid[x + (y * _cols)];
      cell.PreviousState = 0;
      cell.State = 1;
    }

    /// <summary>
    /// Get alive neighbors from cell position.
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    /// <returns>Alive neighbors count</returns>
    protected virtual int GetAliveNeighborsFromCell(int x, int y)
    {
      int count = 0;

      for (int j = -1; j <= 1; ++j)
      {
        for (int i = -1; i <= 1; ++i)
        {
          int cellX = WrapBehavior == WrapBehaviorEnum.Wrap ? Mathf.PosMod(x + i, _cols) : x + i;
          int cellY = WrapBehavior == WrapBehaviorEnum.Wrap ? Mathf.PosMod(y + j, _rows) : y + j;
          count += _grid[cellX + (cellY * _cols)].PreviousState;
        }
      }

      return count - _grid[x + (y * _cols)].PreviousState;
    }

    /// <summary>
    /// Apply rules on cell and return next state.
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="x">Y position</param>
    /// <returns>Next state</returns>
    protected virtual int ApplyRules(int x, int y)
    {
      var neighbors = GetAliveNeighborsFromCell(x, y);
      var state = _grid[x + (y * _cols)].State;

      if (state == 1 && (neighbors < 2 || neighbors > 3))
      {
        return 0;
      }
      else if (state == 0 && neighbors == 3)
      {
        return 1;
      }
      else
      {
        return state;
      }
    }

    /// <summary>
    /// Create a new generation.
    /// </summary>
    protected virtual void Generate()
    {
      int offset = WrapBehavior == WrapBehaviorEnum.Wrap ? 0 : 1;

      for (int j = offset; j < _rows - offset; ++j)
      {
        for (int i = offset; i < _cols - offset; ++i)
        {
          _grid[i + (j * _cols)].PreviousState = _grid[i + (j * _cols)].State;
        }
      }

      for (int j = offset; j < _rows - offset; ++j)
      {
        for (int i = offset; i < _cols - offset; ++i)
        {
          _grid[i + (j * _cols)].State = ApplyRules(i, j);
        }
      }
    }

    private void TryGenerate()
    {
      if (Paused)
      {
        return;
      }

      Generate();
      _generation++;
      UpdateLabel();
    }

    private void TogglePause()
    {
      Paused = !Paused;

      if (Paused)
      {
        _pauseButton.Text = "Touch here to unpause";
      }
      else
      {
        _pauseButton.Text = "Touch here to pause";
      }
    }

    private void UpdateLabel()
    {
      var sb = "Generation: [color=#ffff00]" + _generation + "[/color]";
      _label.BbcodeText = sb;
    }
  }
}
