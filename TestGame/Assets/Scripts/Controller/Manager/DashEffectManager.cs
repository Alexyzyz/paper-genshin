using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectManager : MonoBehaviour
{

    private DashEffect Model = new();

    public static DashEffectManager Instance;

    [SerializeField]
    private DashEffectParticleController dash_effect_particle_prefab;

    private Coroutine draw_effect_coroutine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    public void PlayDashEffect() {
        this.EnsureCoroutineStopped(ref draw_effect_coroutine);
        this.CreateAnimationRoutine(Model.DASH_EFFECT_DURATION, delegate (float t) {
            for (int i = 0; i < Model.DASH_PARTICLE_COUNT_PER_FRAME; i++) {
                float random_angle = Random.Range(0, 2 * Mathf.PI);
                float random_distance = Random.Range(Model.DASH_PARTICLE_MIN_SPAWN_DISTANCE, Model.DASH_PARTICLE_MAX_SPAWN_DISTANCE);

                Vector2 particle_pos_direction = new Vector2(Mathf.Cos(random_angle), Mathf.Sin(random_angle));
                Vector2 particle_start_pos = random_distance * particle_pos_direction;
                Vector2 particle_end_pos = (random_distance + Model.DASH_PARTICLE_MOVE_BACK_DISTANCE) * particle_pos_direction;

                DashEffectParticleController new_particle = Instantiate(dash_effect_particle_prefab, transform, false);
                new_particle.GetComponent<RectTransform>().anchoredPosition = particle_start_pos;
                new_particle.Initialize(particle_start_pos, particle_end_pos, Model.DASH_PARTICLE_ANIMATION_DURATION);
            }
        });
    }
    
}
