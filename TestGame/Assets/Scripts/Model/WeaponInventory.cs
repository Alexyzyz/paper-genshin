using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory
{

    public delegate void WeaponEventHandler(int index, Weapon new_weapon);
    public event WeaponEventHandler OnScrollWeapon;

    private int current_index = 0;

    public List<Weapon> weapon_list = new() { };

    public Weapon GetCurrentWeapon() => weapon_list[current_index];

    public void ScrollNext() {
        current_index = Mathf.Min(current_index + 1, weapon_list.Count - 1);
        OnScrollWeapon?.Invoke(current_index, GetCurrentWeapon());
    }

    public void ScrollPrevious() {
        current_index = Mathf.Max(current_index - 1, 0);
        OnScrollWeapon?.Invoke(current_index, GetCurrentWeapon());
    }

}
