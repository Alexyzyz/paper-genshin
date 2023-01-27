using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    private const float BAR_BORDER_WIDTH = 0.01f;

    private Transform remaining_bar;
    private Transform lag_bar;
    private Transform back_bar;

    private float hp, lag_hp;
    private float lag_speed = 0.2f;

    private void Awake() {
        remaining_bar = transform.Find("Remaining");
        lag_bar = transform.Find("Lag");
        back_bar = transform.Find("Back");
    }

    private void Update() {
        UpdateLagBar();
    }

    public void Initialize(Enemy enemy) {
        SetBarWidth();
        hp = lag_hp = enemy.max_hp;
        enemy.OnUpdateHP += UpdateRemainingBar;
    }

    private void SetBarWidth(float bar_width = 1) {
        Vector3 back_bar_scale = back_bar.localScale;
        back_bar_scale.x = bar_width + 2 * BAR_BORDER_WIDTH;
        back_bar_scale.y = 1 + 2 * BAR_BORDER_WIDTH;

        back_bar.transform.localPosition = new Vector3(-back_bar_scale.x / 2, 0, 0);

        remaining_bar.transform.localPosition = new Vector3(-bar_width / 2, 0, 0);
        lag_bar.transform.localPosition = new Vector3(-bar_width / 2, 0, 0);
    }

    private void UpdateRemainingBar(float hp, float max_hp) {
        hp = Mathf.Max(hp, 0);

        Vector3 remaining_bar_scale = remaining_bar.localScale;
        remaining_bar_scale.x = (back_bar.localScale.x - 2 * BAR_BORDER_WIDTH) * hp / max_hp;
        remaining_bar.localScale = remaining_bar_scale;
    }

    private void UpdateLagBar() {
        float delta_length = lag_bar.localScale.x - remaining_bar.localScale.x;
        float length_decrease = BattleSceneManager.Instance.game_delta_time * lag_speed;

        if (delta_length > length_decrease) {
            lag_bar.localScale -= length_decrease * new Vector3(1, 0, 0);
        } else {
            lag_bar.localScale = remaining_bar.localScale;
        }
    }
    
}
