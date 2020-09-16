using Godot;

namespace Assets
{
  /// <summary>
  /// Contains lazy-loaded materials to use.
  /// </summary>
  public static class SimpleDefaultMaterial
  {
    /// <summary>
    /// Add material.
    /// </summary>
    public static Material AddMaterial
    {
      get
      {
        Initialize();
        return _addMaterial;
      }
    }

    private static Material _addMaterial;

    private static void Initialize()
    {
      if (_addMaterial == null)
      {
        _addMaterial = new CanvasItemMaterial
        {
          BlendMode = CanvasItemMaterial.BlendModeEnum.Add
        };
      }
    }
  }
}
