using System.Collections.Generic;
using UnityEngine;

public class ItemOnMapManager : MonoBehaviour
{
    private Dictionary<int, ItemOnMapBase> _items = new Dictionary<int, ItemOnMapBase>();

    private int currentId = 1;

    public static ItemOnMapManager Instance;
    public GameObject BulletsItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public int GetNextId() => currentId++;

    public BulletsOnMap CreateNewBullets(Vector3 position, WeaponKind bulletsFor, int amount)
    {
        var item = Instantiate(BulletsItemPrefab, position, Quaternion.identity).GetComponent<BulletsOnMap>();

        item.Initialize(GetNextId(), position, bulletsFor, amount);

        _items.Add(item.Id, item);

        return item;
    }

    public void AddNewItem(int id, ItemOnMapBase item)
    {
        _items.Add(id, item);
    }

    public ItemOnMapBase Get(int id)
    {
        return _items[id];
    }

    public void Pickup(int id)
    {
        RoomSendClient.ItemOnMapPickup(id);

        Destroy(_items[id].gameObject);
        _items.Remove(id);
    }
}