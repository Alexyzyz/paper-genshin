using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{

	public WeaponInventory Model { get; } = new();
	[SerializeField]
	private WeaponInventoryView View;

	public static WeaponInventoryManager Instance;

	[SerializeField]
	private List<Weapon> weapon_list;

    private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
    }

    private void Start() {
        Model.OnScrollWeapon += View.ScrollToCurrentWeapon;

        Model.weapon_list = weapon_list;
        View.DrawWeaponInventorySlots(Model.weapon_list);
    }

    private void Update()
	{
        HandleScrollInventory();
    }

	public Weapon GetCurrentWeapon() => Model.GetCurrentWeapon();

	private void HandleScrollInventory() {
		float scroll = Input.mouseScrollDelta.y;
		if (scroll > 0) {
            Model.ScrollPrevious();
		} else if (scroll < 0) {
            Model.ScrollNext();
		}
	}
	
}
