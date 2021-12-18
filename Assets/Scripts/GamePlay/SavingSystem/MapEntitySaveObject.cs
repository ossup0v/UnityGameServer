using UnityEngine;

[RequireComponent(typeof(Transform))]
public class MapEntitySaveObject : MonoBehaviour
{
    private Transform Transform;
    public int MaterialId;
    public string MaterialName;

    private void Awake()
    {
        Transform = GetComponent<Transform>();
    }

    public string Serialize()
    {
        return $"P:{Transform.position.x}!{Transform.position.y}!{Transform.position.z}" +
            $"R:{Transform.rotation.x}!{Transform.rotation.y}!{Transform.rotation.z}!{Transform.rotation.w}" +
            $"S:{Transform.lossyScale.x}!{Transform.lossyScale.y}!{Transform.lossyScale.z}" +
            $"M:{MaterialId};";
    }
}
