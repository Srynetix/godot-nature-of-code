using Godot;
using Assets;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Cellular automata related code.
/// </summary>
namespace Automata
{
  /// <summary>
  /// Cellular automata 1D.
  /// </summary>
  public class CellularAutomata1D : Node2D
  {
    /// <summary>Sent when `_rows` generations have been created.</summary>
    [Signal] public delegate void ScreenCompleted();

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

    /// <summary>Scroll lines?</summary>
    public bool ScrollLines { get; set; }

    /// <summary>Current rule number</summary>
    public byte RuleNumber
    {
      get => _ruleNumber;
      set
      {
        _ruleNumber = value;
        SetRuleSetFromRuleNumber(value);
      }
    }

    private const int RULESET_COUNT = 8;

    private byte _ruleNumber;
    private List<int[]> _lines;
    private readonly int[] _ruleSet;
    private readonly int _scale;
    private int _rows;
    private int _cols;
    private int _generation;
    private Timer _timer;
    private RichTextLabel _label;
    private float _waitTime = 0.05f;

    /// <summary>
    /// Create a default cellular automata with a cell scale of 10.
    /// </summary>
    public CellularAutomata1D() : this(10) { }

    /// <summary>
    /// Create a default cellular automata with a custom cell scale.
    /// </summary>
    /// <param name="scale">Scale</param>
    public CellularAutomata1D(int scale)
    {
      _scale = scale;
      _ruleSet = new int[RULESET_COUNT];
      RuleNumber = 90;
    }

    /// <summary>
    /// Randomize rule set.
    /// </summary>
    public void RandomizeRuleSet()
    {
      RuleNumber = (byte)MathUtils.RandRangei(0, 255);
    }

    /// <summary>
    /// Randomize current line.
    /// </summary>
    public void RandomizeCurrentLine()
    {
      var currRow = GetCurrentRow();
      for (int i = 0; i < _cols; ++i)
      {
        _lines[currRow][i] = MathUtils.RandRangei(0, 1);
      }

      _generation = 0;
    }

    /// <summary>
    /// Reset current line.
    /// </summary>
    public void ResetCurrentLine()
    {
      var currRow = GetCurrentRow();
      for (int i = 0; i < _cols; ++i)
      {
        _lines[currRow][i] = 0;
      }
      _lines[currRow][_cols / 2] = 1;

      _generation = 0;
    }

    public override void _Ready()
    {
      // Create automata on ready
      var size = GetViewportRect().Size;
      _cols = (int)size.x / _scale;
      _rows = (int)size.y / _scale;
      _lines = new List<int[]>();

      // Allocate lines
      var firstLine = new int[_cols];
      firstLine[_cols / 2] = 1;
      _lines.Add(firstLine);
      for (int i = 1; i < _rows; ++i)
      {
        _lines.Add(new int[_cols]);
      }

      // Create timer
      _timer = new Timer
      {
        WaitTime = _waitTime,
        Autostart = true
      };
      AddChild(_timer);
      _timer.Connect("timeout", this, nameof(Generate));

      // Create label
      var font = SimpleDefaultFont.Regular;
      var textSize = font.GetStringSize(GenerateRuleSetString(includeBbCode: false));
      _label = new RichTextLabel
      {
        BbcodeEnabled = true,
        ScrollActive = false
      };
      _label.RectGlobalPosition = new Vector2(8, (size.y / 2) - textSize.y);
      _label.RectMinSize = new Vector2(size.x / 2, size.y);
      _label.Set("custom_fonts/normal_font", font);
      AddChild(_label);
    }

    public override void _Process(float delta)
    {
      Update();
    }

    public override void _Draw()
    {
      for (int j = 0; j < _rows; ++j)
      {
        for (int i = 0; i < _cols; ++i)
        {
          if (_lines[j][i] == 1)
          {
            DrawRect(new Rect2(i * _scale, j * _scale, _scale, _scale), CellColor);
          }
        }
      }
    }

    private void Generate()
    {
      int currRow;
      int nextRow;

      if (ScrollLines)
      {
        if (_generation > _rows)
        {
          ScrollLinesDown();
        }

        currRow = Mathf.Min(_rows - 2, _generation);
        nextRow = Mathf.Min(_rows - 1, _generation + 1);
      }
      else
      {
        currRow = _generation % _rows;
        nextRow = (currRow + 1) % _rows;
      }

      _lines[nextRow] = GenerateRow(currRow);
      _generation++;

      UpdateLabel();

      if (_generation % _rows == 0)
      {
        EmitSignal(nameof(ScreenCompleted));
      }
    }

    private int[] GenerateRow(int currentRow)
    {
      int[] nextgen = new int[_cols];

      for (int i = 1; i < _cols - 1; ++i)
      {
        int left = _lines[currentRow][i - 1];
        int curr = _lines[currentRow][i];
        int right = _lines[currentRow][i + 1];
        nextgen[i] = ApplyRules1d(left, curr, right);
      }

      return nextgen;
    }

    private int GetCurrentRow()
    {
      if (ScrollLines)
      {
        return Mathf.Min(_rows - 1, _generation);
      }
      else
      {
        return _generation % _rows;
      }
    }

    private void UpdateLabel()
    {
      _label.BbcodeText = "Generation: [color=#ffff00]" + _generation + "[/color]\n" + GenerateRuleSetString();
    }

    private string GenerateRuleSetString(bool includeBbCode = true)
    {
      var sb = new StringBuilder();
      sb.Append("Ruleset: ");
      if (includeBbCode)
      {
        sb.Append("[color=#00ff00]");
      }
      sb.Append("Rule ");
      sb.Append(_ruleNumber);
      if (includeBbCode)
      {
        sb.Append("[/color]");
      }
      return sb.ToString();
    }

    private void SetRuleSetFromRuleNumber(byte number)
    {
      var binaryString = Convert.ToString(number, 2).PadLeft(8, '0');
      var binaryStringArray = binaryString.ToCharArray();
      for (int i = 0; i < RULESET_COUNT; ++i)
      {
        _ruleSet[RULESET_COUNT - 1 - i] = binaryStringArray[i] - '0';
      }
    }

    private void ScrollLinesDown()
    {
      _lines.RemoveAt(0);
      _lines.Add(new int[_cols]);
    }

    private int ApplyRules1d(int leftState, int currState, int rightState)
    {
      var sb = new StringBuilder(3);
      sb.Append(leftState);
      sb.Append(currState);
      sb.Append(rightState);
      int idx = Convert.ToInt32(sb.ToString(), 2);
      return _ruleSet[idx];
    }
  }
}
