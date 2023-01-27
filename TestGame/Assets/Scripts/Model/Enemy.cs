using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{

    public delegate void HPEventHandler(float hp, float max_hp);
    public event HPEventHandler OnUpdateHP;

    public delegate void ElementEventHandler(GameData.Element element);
    public event ElementEventHandler OnElementApplied;

    public delegate void ReactionEventHandler(GameData.Reaction reaction);
    public event ReactionEventHandler OnReactionTriggered;

    public delegate void FrozenEventHandler();
    public event FrozenEventHandler OnUnfrozen;

    private float hp;
    public float max_hp { get; }

    public float MIN_SPEED { get; } = 1.5f;
    public float MAX_SPEED { get; } = 3.5f;
    public float BASE_SPEED { get; private set; }

    public float DAMAGE_TEXT_ALTITUDE { get; } = 2f;
    public float DAMAGE_TEXT_OFFSET_RANGE { get; } = 0.1f;

    public GameData.Element element { get; private set; }
    private const float ELEMENT_DURATION = 10f;
    private float element_timer;

    public float FROZEN_DURATION { get; } = 6f;
    public float frozen_timer { get; private set; } = 0;

    public float SWIRL_RADIUS { get; } = 15f;

    public float HP {
        get { return hp; }
        set {
            hp = Mathf.Max(value, 0);
            OnUpdateHP?.Invoke(hp, max_hp);
        }
    }

    public void SetBaseSpeed() => BASE_SPEED = Random.Range(MIN_SPEED, MAX_SPEED);

    public void HandleTakeDamage(float damage) => HP -= damage;

    public GameData.Reaction ApplyElement(GameData.Element new_element) {
        if (element == GameData.Element.NONE) {
            if (new_element == GameData.Element.ANEMO) {
                return GameData.Reaction.NONE;
            }

            element = new_element;
            element_timer = ELEMENT_DURATION;

            OnElementApplied?.Invoke(element);
            return GameData.Reaction.NONE;
        }

        if (element == new_element) {
            return GameData.Reaction.NONE;
        }

        GameData.Reaction triggered_reaction = ElementUtility.Instance.GetReaction(element, new_element);
        RemoveElement();
        OnReactionTriggered?.Invoke(triggered_reaction);
        return triggered_reaction;
    }

    public void RemoveElement() {
        element = GameData.Element.NONE;
        element_timer = 0;
        OnElementApplied?.Invoke(GameData.Element.NONE);
    }

    public void SetFrozen() => frozen_timer = FROZEN_DURATION;

    public void TickDownElementDuration() {
        element_timer -= BattleSceneManager.Instance.game_delta_time;
        if (element != GameData.Element.NONE && element_timer <= 0) {
            element = 0;
            RemoveElement();
        }
    }

    public void TickDownFrozenDuration() {
        frozen_timer -= BattleSceneManager.Instance.game_delta_time;
        if (frozen_timer < 0) {
            frozen_timer = 0;
            OnUnfrozen?.Invoke();
        }
    }

    public Enemy(float hp = 100, float max_hp = 100) {
        this.hp = hp;
        this.max_hp = max_hp;
    }

}
