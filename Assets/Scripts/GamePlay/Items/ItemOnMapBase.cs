using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ItemOnMapBase : MonoBehaviour
{
    public int Id { get; private set; }
    public ItemOnMapKind Kind { get; private set; }

    public Vector3 Position { get; private set; }

    public void Initialize(int id, Vector3 position, ItemOnMapKind kind)
    {
        Id = id;
        Position = position;
        Kind = kind;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            if (TryPickup(player))
            {
                ItemOnMapManager.Instance.Pickup(Id);

                Destroy(gameObject);
            }
        }
    }

    public abstract bool TryPickup(CharacterBase characterBase);
}
