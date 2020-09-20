using Godot;

namespace Assets
{
  /// <summary>
  /// Contains lazy-loaded textures to use.
  /// </summary>
  public static class SimpleDefaultTexture
  {
    /// <summary>Default white circle texture with black background.</summary>
    public static Texture WhiteDotTexture
    {
      get
      {
        Initialize();
        return _whiteDotTexture;
      }
    }

    /// <summary>Default white circle texture with alpha background.</summary>
    public static Texture WhiteDotAlphaTexture
    {
      get
      {
        Initialize();
        return _whiteDotAlphaTexture;
      }
    }

    /// <summary>Default white circle texture with outline and alpha background.</summary>
    public static Texture WhiteDotAlphaWithOutlineTexture
    {
      get
      {
        Initialize();
        return _whiteDotAlphaWithOutlineTexture;
      }
    }

    /// <summary>Default white circle outline with alpha background.</summary>
    public static Texture WhiteDotOutlineOnlyTexture
    {
      get
      {
        Initialize();
        return _whiteDotOutlineOnlyTexture;
      }
    }

    /// <summary>Default white blurry circle with alpha background.</summary>
    public static Texture WhiteDotBlurTexture
    {
      get
      {
        Initialize();
        return _whiteDotBlurTexture;
      }
    }

    /// <summary>White hexagon texture.</summary>
    public static Texture HexagonTexture
    {
      get
      {
        Initialize();
        return _hexagonTexture;
      }
    }

    /// <summary>Right arrow with alpha background</summary>
    public static Texture RightArrowTexture
    {
      get
      {
        Initialize();
        return _rightArrowTexture;
      }
    }

    /// <summary>Default vertical line texture.</summary>
    public static Texture LineTexture
    {
      get
      {
        Initialize();
        return _lineTexture;
      }
    }

    private static Texture _whiteDotTexture;
    private static Texture _whiteDotAlphaTexture;
    private static Texture _whiteDotAlphaWithOutlineTexture;
    private static Texture _whiteDotOutlineOnlyTexture;
    private static Texture _whiteDotBlurTexture;
    private static Texture _hexagonTexture;
    private static Texture _rightArrowTexture;
    private static Texture _lineTexture;

    private static void Initialize()
    {
      if (_whiteDotTexture == null)
      {
        _whiteDotTexture = (Texture)GD.Load("res://assets/textures/white-dot-on-black.png");
      }

      if (_whiteDotBlurTexture == null)
      {
        _whiteDotBlurTexture = (Texture)GD.Load("res://assets/textures/white-dot-blur.png");
      }

      if (_whiteDotAlphaTexture == null)
      {
        _whiteDotAlphaTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha.png");
      }

      if (_whiteDotOutlineOnlyTexture == null)
      {
        _whiteDotOutlineOnlyTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha-outline.png");
      }

      if (_whiteDotAlphaWithOutlineTexture == null)
      {
        _whiteDotAlphaWithOutlineTexture = (Texture)GD.Load("res://assets/textures/white-dot-alpha-with-outline.png");
      }

      if (_hexagonTexture == null)
      {
        _hexagonTexture = (Texture)GD.Load("res://assets/textures/hexagon.png");
      }

      if (_rightArrowTexture == null)
      {
        _rightArrowTexture = (Texture)GD.Load("res://assets/textures/arrow-right.png");
      }

      if (_lineTexture == null)
      {
        _lineTexture = (Texture)GD.Load("res://assets/textures/line.png");
      }
    }
  }
}
