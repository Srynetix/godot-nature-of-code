using Godot;

namespace Assets
{
  /// <summary>
  /// Contains lazy-loaded materials to use.
  /// </summary>
  public static class SimpleDefaultMaterial
  {
    /// <summary>
    /// Material types.
    /// </summary>
    public enum Enum
    {
      /// <summary>Default material with BlendMode.Add</summary>
      Add
    }

    private static Material AddMaterial;

    /// <summary>
    /// Get or create a default material from an enum value.
    /// </summary>
    /// <param name="value">Material enum value</param>
    /// <returns>Generated material</returns>
    public static Material FromEnum(Enum value)
    {
      Initialize();

      if (value == Enum.Add)
      {
        return AddMaterial;
      }

      return null;
    }

    private static void Initialize()
    {
      if (AddMaterial == null)
      {
        var material = new CanvasItemMaterial();
        material.BlendMode = CanvasItemMaterial.BlendModeEnum.Add;
        AddMaterial = material;
      }
    }
  }
}
