using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour
{

    private const float SPAWN_RANDOM_POS_RANGE = 10f;

    private RectTransform rect_transform;
    private Text text_component;
    private CanvasGroup canvas_group;

    private Vector3 world_space_position;

    private Coroutine animation_coroutine;

    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        text_component = GetComponent<Text>();
        canvas_group = GetComponent<CanvasGroup>();
    }

    private void Update() {
        UpdateCanvasPosition();
    }

    public void Initialize(string text, Color color, int font_size, Vector3 world_space_position) {
        this.world_space_position = world_space_position;

        text_component.text = text;
        text_component.color = color;
        text_component.fontSize = font_size;
        FadeIn();
    }

    private void UpdateCanvasPosition() {
        transform.position = Camera.main.WorldToScreenPoint(world_space_position);
    }

    private void FadeIn() {
        animation_coroutine = this.CreateAnimationRoutine(
            0.1f,
            delegate (float t) {
                float scale_scalar = Mathf.Lerp(2, 1, t);
                float alpha = Mathf.Lerp(0, 1, t);

                rect_transform.localScale = Vector2.one * scale_scalar;
                canvas_group.alpha = alpha;
            },
            delegate () {
                this.CreateTimerRoutine(0.5f, FadeOut);
            }
        );
    }

    private void FadeOut() {
        this.EnsureCoroutineStopped(ref animation_coroutine);
        animation_coroutine = this.CreateAnimationRoutine(
            0.1f,
            delegate (float t) {
                float scale_scalar = Mathf.Lerp(1, 0, t);
                float alpha = Mathf.Lerp(1, 0, t);

                rect_transform.localScale = Vector2.one * scale_scalar;
                canvas_group.alpha = alpha;
            },
            delegate () {
                Destroy(gameObject);
            }
        );
    }
    
}
