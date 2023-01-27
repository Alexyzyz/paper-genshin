using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Player Model { get; } = new();

    [SerializeField]
    private List<AudioClip> dash_sound_list;
    [SerializeField]
    private AnimationCurve dash_speed_decay_curve;

    [SerializeField]
    private GameObject shot_effect_prefab;

    private CharacterController character_controller;
    private CameraController camera_controller;
    private AudioSource audio_source;

    private Vector3 speed_vector;

    private Vector3 dash_direction_vector;
    private float dash_speed;
    private int dash_sound_index;

    private Coroutine dash_speed_decay_coroutine;

    private void Awake() {
        character_controller = GetComponent<CharacterController>();
        camera_controller = transform.Find("Camera").GetComponent<CameraController>();
        audio_source = GetComponent<AudioSource>();
    }

    private void Update() {
        HandlePlanarMovement();
        HandleJump();
        HandleWeaponAim();
        HandleShoot();

        UpdateMovement();

        Model.TickDownDashTimer();
        Model.RefreshMana();
    }

    private void HandleShoot() {
        if (!Input.GetKeyDown(GameKey.SHOOT)) return;

        Weapon current_weapon = WeaponInventoryManager.Instance.GetCurrentWeapon();
        if (current_weapon.mana_cost > Model.mana) return;

        Model.SpendManaOnShot(current_weapon);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (!Physics.Raycast(ray, out RaycastHit raycast_data)) return;
        Collider hit_collider = raycast_data.collider;

        if (!hit_collider) return;
        GameObject shot_effect = Instantiate(shot_effect_prefab, raycast_data.point, Quaternion.identity, transform);
        shot_effect.transform.LookAt(transform);

        if (hit_collider.gameObject.CompareTag("Enemy")) {
            EnemyController hit_enemy = hit_collider.gameObject.GetComponent<EnemyController>();
            hit_enemy.HandleHit(new Attack(current_weapon.damage, current_weapon.element));
        }
    }

    private void HandleWeaponAim() {
        if (Input.GetKeyDown(GameKey.AIM)) {
            camera_controller.SetAiming(true);
        } else if (Input.GetKeyUp(GameKey.AIM)) {
            camera_controller.SetAiming(false);
        }
    }

    private void UpdateMovement() {
        character_controller.Move(delta_time * speed_vector);

        speed_vector.y -= delta_time * (character_controller.isGrounded ? 0 : Model.GRAVITY);
        speed_vector.y = Mathf.Clamp(speed_vector.y, -Model.MAX_Y_SPEED, Model.MAX_Y_SPEED);
    }

    private void HandlePlanarMovement() {
        int x_axis = Input.GetKey(GameKey.RIGHT) ? 1 : 0 - (Input.GetKey(GameKey.LEFT) ? 1 : 0);
        int z_axis = Input.GetKey(GameKey.UP) ? 1 : 0 - (Input.GetKey(GameKey.DOWN) ? 1 : 0);

        transform.eulerAngles = new Vector3(0, camera_controller.GetYAngle(), 0);

        Vector3 planar_direction_vector = (transform.forward * z_axis + transform.right * x_axis).normalized;
        HandleDash(planar_direction_vector);

        Vector3 planar_speed_vector = Model.BASE_SPEED * planar_direction_vector + dash_speed * dash_direction_vector;

        speed_vector.x = planar_speed_vector.x;
        speed_vector.z = planar_speed_vector.z;
    }

    private void HandleJump() {
        if (!Input.GetKeyDown(GameKey.JUMP)) return;
        if (!character_controller.isGrounded) return;

        speed_vector.y = Model.JUMP_HEIGHT;
    }

    private void HandleDash(Vector3 planar_direction_vector) {
        if (!Input.GetKeyDown(GameKey.DASH)) return;
        if (Model.dash_timer > 0) return;

        dash_direction_vector = planar_direction_vector.magnitude == 0 ? transform.forward : planar_direction_vector;
        dash_speed = Model.BASE_DASH_SPEED;

        DashEffectManager.Instance.PlayDashEffect();
        audio_source.PlayOneShot(dash_sound_list[dash_sound_index]);
        dash_sound_index = (dash_sound_index + 1) % dash_sound_list.Count;

        Model.ResetDashCooldownTimer();

        this.EnsureCoroutineStopped(ref dash_speed_decay_coroutine);
        dash_speed_decay_coroutine = this.CreateAnimationRoutine(
            Model.DASH_DURATION,
            delegate (float _t) {
                float t = 1 - dash_speed_decay_curve.Evaluate(_t);
                dash_speed = Mathf.Lerp(Model.BASE_DASH_SPEED, 0, t);
            },
            delegate () {
                dash_speed = 0;
            }
        );

    }

    private float delta_time => BattleSceneManager.Instance.game_delta_time;

}
