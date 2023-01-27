using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    private Enemy Model = new();

    private HealthBarController health_bar_controller;
    private ElementStatusController element_status_controller;

    private SpriteRenderer sprite_renderer;
    private NavMeshAgent nav_mesh_agent;

    [SerializeField]
    private Sprite normal_sprite;
    [SerializeField]
    private Sprite frozen_sprite;

    private void Awake() {
        health_bar_controller = transform.Find("HealthBar").GetComponent<HealthBarController>();
        element_status_controller = transform.Find("ElementStatus").GetComponent<ElementStatusController>();

        sprite_renderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        nav_mesh_agent = GetComponent<NavMeshAgent>();

        sprite_renderer.sprite = normal_sprite;

        Model.SetBaseSpeed();
    }

    private void Start() {
        health_bar_controller.Initialize(Model);

        Model.OnElementApplied += element_status_controller.HandleUpdateElement;
        Model.OnReactionTriggered += element_status_controller.AnimateTriggeredReaction;
        
        Model.OnUnfrozen += HandleOnUnfrozen;

        Model.OnUpdateHP += HandleDeath;
    }

    private void Update()
    {
        FaceTowardPlayer();

        Model.TickDownElementDuration();
        Model.TickDownFrozenDuration();
    }

    private void FixedUpdate() {
        HandleNavigation();
    }

    public void HandleHit(Attack attack_data) {
        GameData.Element previous_element = Model.element;
        GameData.Reaction triggered_reaction = Model.ApplyElement(attack_data.element);

        if (triggered_reaction != GameData.Reaction.NONE) {
            string triggered_reaction_name = ElementUtility.Instance.GetReactionName(triggered_reaction);
            DrawDamageText(triggered_reaction_name, attack_data.element, true);
        }

        float final_damage = attack_data.damage;
        if (triggered_reaction == GameData.Reaction.VAPORIZE) {
            final_damage *= 2;
        } else
        if (triggered_reaction == GameData.Reaction.MELT) {
            final_damage *= 2;
        } else
        if (triggered_reaction == GameData.Reaction.FROZEN) {
            Model.SetFrozen();
            HandleOnFrozen();
        } else
        if (triggered_reaction == GameData.Reaction.SWIRL) {
            List<EnemyController> enemy_list = BattleSceneManager.Instance.enemy_controller_list;
            Attack swirl_attack_data = new Attack(attack_data.damage, previous_element);

            foreach (EnemyController enemy in enemy_list) {
                if (enemy == this) continue;
                if ((enemy.transform.position - transform.position).magnitude > Model.SWIRL_RADIUS) continue;
                enemy.HandleHit(swirl_attack_data);
            }
        }

        Model.HandleTakeDamage(final_damage);
        DrawDamageText("" + final_damage, attack_data.element, false);
    }

    private void HandleDeath(float hp, float max_hp) {
        if (hp > 0) return;
        BattleSceneManager.Instance.HandleEnemyDeath(this);
        Destroy(gameObject);
    }

    private void HandleOnFrozen() => sprite_renderer.sprite = frozen_sprite;

    private void HandleOnUnfrozen() => sprite_renderer.sprite = normal_sprite;

    private void HandleNavigation() {
        if (Model.frozen_timer > 0) {
            nav_mesh_agent.speed = 0;
            return;
        }
        nav_mesh_agent.speed = Model.BASE_SPEED;
        nav_mesh_agent.destination = BattleSceneManager.Instance.player_controller.transform.position;
    }

    private void DrawDamageText(string text, GameData.Element element, bool is_reaction) {
        float offset_range = is_reaction ? 0.8f : 0.1f;
        int font_size = is_reaction ? 48 : 64;

        Color damage_color = ElementUtility.Instance.GetElementColor(element);
        if (is_reaction) {
            damage_color.a = 0.8f;
        }

        Vector3 damage_text_root_pos = transform.position + new Vector3(0, Model.DAMAGE_TEXT_ALTITUDE, 0);
        Vector3 damage_text_number_pos = damage_text_root_pos + GetRandomVector(offset_range);
        DamageTextManager.Instance.DrawDamageText(text, damage_color, font_size, damage_text_number_pos);
    }

    private void FaceTowardPlayer() {
        Vector3 player_planar_pos = BattleSceneManager.Instance.player_controller.transform.position;
        player_planar_pos.y = transform.position.y;
        transform.LookAt(player_planar_pos);
    }

    private Vector3 GetRandomVector(float offset_range) {
        float x_offset = Random.Range(-offset_range, offset_range);
        float y_offset = Random.Range(-offset_range, offset_range);
        float z_offset = Random.Range(-offset_range, offset_range);
        return new Vector3(x_offset, y_offset, z_offset);
    }
    
}
