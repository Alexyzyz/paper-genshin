using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryView : MonoBehaviour
{

    private const float INVENTORY_SLOT_WIDTH = 150;
    private const float INVENTORY_SLOT_MARGIN = 10;
    private const float AUTO_HIDE_TIME = 2f;

    [SerializeField]
    private GameObject weapon_inventory_slot_prefab;

    private RectTransform row_rect;
    private Text weapon_description_text_component;
    private CanvasGroup canvas_group;

    private List<WeaponInventorySlotView> weapon_inventory_slot_view_list = new();

    private Coroutine toggle_show_coroutine;
    private Coroutine auto_hide_timer_coroutine;
    private Coroutine scroll_to_current_weapon_coroutine;

    private void Awake() {
        row_rect = transform.Find("Row").GetComponent<RectTransform>();
        weapon_description_text_component = transform.Find("WeaponDescription").GetComponent<Text>();
        canvas_group = GetComponent<CanvasGroup>();
    }

    public void Show() {
        AnimateAlphaTo(1);
        this.EnsureCoroutineStopped(ref auto_hide_timer_coroutine);
        auto_hide_timer_coroutine = this.CreateTimerRoutine(AUTO_HIDE_TIME, Hide);
    }

    public void Hide() => AnimateAlphaTo(0);

    public void ScrollToCurrentWeapon(int current_index, Weapon current_weapon) {
        Show();
        AnimateScrollToCurrentWeapon(current_index);
        weapon_description_text_component.text = "<b>" + current_weapon.title + "</b>\n" + current_weapon.description;

        foreach (WeaponInventorySlotView weapon_inventory_slot_view in weapon_inventory_slot_view_list) {
            weapon_inventory_slot_view.UpdateSelectedState(current_index);
        }
    }

    public void DrawWeaponInventorySlots(List<Weapon> weapon_list) {
        for (int i = 0; i < weapon_list.Count; i++) {
            Vector2 pos = new(i * (INVENTORY_SLOT_WIDTH + INVENTORY_SLOT_MARGIN), 0);
            WeaponInventorySlotView new_slot = Instantiate(weapon_inventory_slot_prefab, row_rect, false).GetComponent<WeaponInventorySlotView>();
            new_slot.GetComponent<RectTransform>().anchoredPosition = pos;
            new_slot.Initialize(i, weapon_list[i], i == 0);
            weapon_inventory_slot_view_list.Add(new_slot);
        }
    }

    private void AnimateAlphaTo(float target_alpha) {
        this.EnsureCoroutineStopped(ref toggle_show_coroutine);
        float start_alpha = canvas_group.alpha;
        toggle_show_coroutine = this.CreateAnimationRoutine(
            0.1f,
            delegate (float t) {
                canvas_group.alpha = Mathf.Lerp(start_alpha, target_alpha, t);
            }
        );
    }

    private void AnimateScrollToCurrentWeapon(int current_weapon_index) {
        this.EnsureCoroutineStopped(ref scroll_to_current_weapon_coroutine);
        float start_x = row_rect.anchoredPosition.x;
        float end_x = -current_weapon_index * (INVENTORY_SLOT_WIDTH + INVENTORY_SLOT_MARGIN);
        scroll_to_current_weapon_coroutine = this.CreateAnimationRoutine(
            0.1f,
            delegate (float t) {
                float new_x = Mathf.Lerp(start_x, end_x, t);
                Vector2 new_pos = row_rect.anchoredPosition;
                new_pos.x = new_x;
                row_rect.anchoredPosition = new_pos;
            }
        );
    }

}
