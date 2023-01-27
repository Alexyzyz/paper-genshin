using System;
using System.Collections;
using UnityEngine;

public static class CoroutineManager
{
    public static void EnsureCoroutineStopped(this MonoBehaviour value, ref Coroutine routine) {
        if (routine != null) {
            value.StopCoroutine(routine);
            routine = null;
        }
    }

    public static Coroutine CreateTimerRoutine(this MonoBehaviour value, float duration, Action on_complete = null) {
        return value.StartCoroutine(TimerRoutine(duration, on_complete));
    }

    private static IEnumerator TimerRoutine(float duration, Action on_complete) {
        yield return new WaitForSeconds(duration);
        on_complete?.Invoke();
    }

    public static Coroutine CreateAnimationRoutine(this MonoBehaviour value, float duration, Action<float> transformation_function, Action on_complete = null) {
        return value.StartCoroutine(AnimationRoutine(duration, transformation_function, on_complete));
    }

    private static IEnumerator AnimationRoutine(float duration, Action<float> transformation_function, Action on_complete) {
        float elapsed_time = 0;
        float progress = 0;
        while (progress <= 1) {
            transformation_function(progress);
            elapsed_time += BattleSceneManager.Instance.game_delta_time;
            progress = elapsed_time / duration;
            yield return null;
        }
        transformation_function(1);
        on_complete?.Invoke();
    }

}
