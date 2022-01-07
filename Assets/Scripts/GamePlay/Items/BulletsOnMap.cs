using UnityEngine;

public sealed class BulletsOnMap : ItemOnMapBase
{
    public WeaponKind BulletsFor { get; private set; }
    public int Amount { get; private set; }

    public void Initialize(int id, Vector3 position, WeaponKind bulletsFor, int amount)
    {
        base.Initialize(id, position, ItemOnMapKind.bullets);
        BulletsFor = bulletsFor;
        Amount = amount;
    }

    public override bool TryPickup(CharacterBase character)
    {
        return character.TryPickupBullets(this);
    }
}
