using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    private const float ALTITUDE = 80f;
    private const float ORBIT_RADIUS = 50f;
    private const float ORBIT_DURATION = 60f;

    private Coroutine light_movement_coroutine;

    private void Start()
    {
        MoveLight();
    }

    private void MoveLight() {
        this.EnsureCoroutineStopped(ref light_movement_coroutine);
        light_movement_coroutine = this.CreateAnimationRoutine(
            ORBIT_DURATION,
            delegate (float _t) {
                float t = _t * 2 * Mathf.PI;
                Vector3 new_pos = ORBIT_RADIUS * new Vector3(Mathf.Cos(t), 0, Mathf.Sin(t));
                new_pos.y = ALTITUDE;
                transform.position = new_pos;
                transform.LookAt(Vector3.zero);
            },
            delegate () {
                MoveLight();
            }
        );
    }
    
}
