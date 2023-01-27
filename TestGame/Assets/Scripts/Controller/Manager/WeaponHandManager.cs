using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHandManager : MonoBehaviour
{

    public static WeaponHandManager Instance;

    private Image image_component;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        image_component = GetComponent<Image>();
    }

    private void Start() {
        WeaponInventoryManager.Instance.Model.OnScrollWeapon += HandleWeaponChange;
    }

    public void HandleWeaponChange(int current_index, Weapon new_weapon) {
        image_component.sprite = new_weapon.sprite;
    }

}
