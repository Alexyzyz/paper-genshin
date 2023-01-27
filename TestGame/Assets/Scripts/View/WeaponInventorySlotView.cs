using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlotView : MonoBehaviour
{

    private const float UNSELECTED_SCALE = 0.8f;
    private const float UNSELECTED_ALPHA = 0.5f;

    private RectTransform rect;
    private CanvasGroup canvas_group;
    private Image weapon_icon_image;

    private int index;

    private Coroutine toggle_selected_coroutine;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
        weapon_icon_image = transform.Find("Icon").GetComponent<Image>();
    }

    public void Initialize(int index, Weapon weapon, bool is_starting_index = false) {
        this.index = index;
        weapon_icon_image.sprite = weapon.sprite;

        if (!is_starting_index) {
            rect.localScale = UNSELECTED_SCALE * Vector3.one;
            canvas_group.alpha = UNSELECTED_ALPHA;
        }
    }

    public void UpdateSelectedState(int current_index) => AnimateSelectedState(current_index == index);

    private void AnimateSelectedState(bool is_selected) {
        this.EnsureCoroutineStopped(ref toggle_selected_coroutine);
        
        float start_scale = transform.localScale.x;
        float end_scale = is_selected ? 1 : UNSELECTED_SCALE;

        float start_alpha = canvas_group.alpha;
        float end_alpha = is_selected ? 1 : UNSELECTED_ALPHA;

        this.CreateAnimationRoutine(
            0.1f,
            delegate (float t) {
                float new_scale = Mathf.Lerp(start_scale, end_scale, t);
                float new_alpha = Mathf.Lerp(start_alpha, end_alpha, t);
                rect.localScale = new_scale * Vector3.one;
                canvas_group.alpha = new_alpha;
            }
        );
    }
    
}
