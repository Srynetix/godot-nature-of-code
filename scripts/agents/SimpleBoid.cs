using Godot;
using Drawing;
using Assets;

namespace Agents
{
  /// <summary>
  /// Simple vehicle configured as a boid.
  /// </summary>
  public class SimpleBoid : SimpleVehicle
  {
    /// <summary>
    /// Create a default boid.
    /// </summary>
    public SimpleBoid()
    {
      SeparationEnabled = true;
      CohesionEnabled = true;
      AlignmentEnabled = true;

      SeparationForceFactor = 1.5f;
      AlignmentForceFactor = 1f;
      CohesionForceFactor = 1f;
      MaxForce = 0.1f;

      Mesh.MeshType = SimpleMesh.TypeEnum.Texture;
      Mesh.CustomTexture = SimpleDefaultTexture.RightArrowTexture;
      Mesh.Modulate = Colors.White;
      Mesh.MeshSize = new Vector2(20, 10);

      Name = "SimpleBoid";
    }
  }
}
