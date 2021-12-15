using System.Collections;
using System.Collections.Generic;
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
}
