using Godot;
using Drawing;

namespace Agents
{
  /// <summary>
  /// Vehicle with a round shape.
  /// </summary>
  public class RoundVehicle : SimpleVehicle
  {
    /// <summary>
    /// Create a default round vehicle.
    /// </summary>
    public RoundVehicle()
    {
      Mesh.MeshType = SimpleMesh.TypeEnum.Circle;
      Mesh.MeshSize = new Vector2(10, 10);
    }
  }
}
