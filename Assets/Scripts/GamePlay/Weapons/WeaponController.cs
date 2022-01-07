using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController
{
    private List<WeaponBase> weapons;
    private int weaponCurrentIndex;

    public WeaponController(List<WeaponBase> weapons)
    {
        this.weapons = weapons;
    }

    public WeaponBase GetCurrentWeapon()
    {
        return weapons[weaponCurrentIndex];
    }

    public WeaponBase TryChooseWeaponByIndex(int index, out bool result)
    {
        result = false;

        if (index < 0)
        {
            return GetCurrentWeapon();
        }

        if (weapons.Count > index)
        {
            weaponCurrentIndex = index;
            result = true;
        }

        return GetCurrentWeapon();
    }

    public WeaponBase ChangeWeapon(int leftOrRigth)
    {
        var weaponCount = weapons.Count - 1;

        if (leftOrRigth > 0)
        {
            if (weaponCurrentIndex == weaponCount)
            {
                weaponCurrentIndex = 0;
                return weapons[weaponCurrentIndex];
            }

            weaponCurrentIndex++;
            return weapons[weaponCurrentIndex];
        }
        else
        {
            if (weaponCurrentIndex == 0)
            {
                weaponCurrentIndex = weaponCount;
                return weapons[weaponCurrentIndex];
            }

            weaponCurrentIndex--;
            return weapons[weaponCurrentIndex];
        }
    }

    public WeaponBase TryAddBullets(BulletsOnMap bulletsOnMap, out bool result)
    {
        var targetWeapon = weapons.FirstOrDefault(x => x.Kind == bulletsOnMap.BulletsFor);

        result = false;
        if (targetWeapon == null)
            return null;

        if (targetWeapon.IsBulletsFull)
            return null;
        
        result = true;

        targetWeapon.AddBullets(bulletsOnMap.Amount);
        return targetWeapon;
    }
}
