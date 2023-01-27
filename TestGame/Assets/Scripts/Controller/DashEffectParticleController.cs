using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectParticleController : MonoBehaviour
{

    private RectTransform rect_transform;
    private CanvasGroup canvas_group;

    private Coroutine animation_coroutine;

    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
    }

    public void Initialize(Vector2 start_pos, Vector2 end_pos, float animation_duration) {
        Vector2 direction_vector = start_pos - end_pos;
        transform.up = direction_vector.normalized;
        AnimateMe(start_pos, end_pos, animation_duration);
    }

    private void AnimateMe(Vector2 start_pos, Vector2 end_pos, float animation_duration) {
        float start_alpha = 0.2f;
        float end_alpha = 1f;

        float start_scale = 0.8f;
        float end_scale = 1f;

        animation_coroutine = this.CreateAnimationRoutine(
            animation_duration,
            delegate (float t) {
                canvas_group.alpha = Mathf.Lerp(start_alpha, end_alpha, t);
            
                float t_scale = Mathf.Lerp(start_scale, end_scale, t);
                rect_transform.localScale = t_scale * Vector2.one;

                rect_transform.anchoredPosition = Vector2.Lerp(start_pos, end_pos, t);
            },
            delegate () {
                this.EnsureCoroutineStopped(ref animation_coroutine);
                Destroy(gameObject);
            }
        );
    }
    
}
