using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager Instance;

    private CanvasGroup canvas_group;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        canvas_group = GetComponent<CanvasGroup>();
    }

    public void HideTutorial() {
        this.CreateAnimationRoutine(
            0.5f,
            delegate (float t) {
                float alpha = Mathf.Lerp(1, 0, t);
                canvas_group.alpha = alpha;
            }
        );
    }

}
