using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public delegate void ManaEventHandler(float mana, float max_mana);
    public event ManaEventHandler OnManaChange;

    public const float MAX_MANA = 100f;
    public float MANA_INCREASE { get; } = 10f;
    public float mana = MAX_MANA;

    public float BASE_SPEED { get; } = 5f;
    public float AIMING_BASE_SPEED_RATIO { get; } = 0.8f;
    
    public float GRAVITY { get; } = 30f;
    public float JUMP_HEIGHT { get; } = 10f;
    public float MAX_Y_SPEED { get; } = 15f;

    public float BASE_DASH_SPEED { get; } = 15f;
    public float DASH_SPEED_DECAY_LERP_RATIO { get; } = 0.01f;
    public float DASH_DURATION { get; } = 0.75f;
    public float DASH_COOLDOWN { get; } = 1f;
    public float dash_timer { get; private set; }

    public void ResetDashCooldownTimer() => dash_timer = DASH_COOLDOWN;

    public void TickDownDashTimer() {
        dash_timer -= BattleSceneManager.Instance.game_delta_time;
        dash_timer = Mathf.Max(dash_timer, 0);
    }

    public void RefreshMana() {
        if (mana == MAX_MANA) return;
        mana += BattleSceneManager.Instance.game_delta_time * MANA_INCREASE;
        mana = Mathf.Min(mana, MAX_MANA);
        OnManaChange?.Invoke(mana, MAX_MANA);
    }

    public void SpendManaOnShot(Weapon weapon) {
        if (mana < weapon.mana_cost) return;
        mana -= weapon.mana_cost;
        OnManaChange?.Invoke(mana, MAX_MANA);
    }

}
