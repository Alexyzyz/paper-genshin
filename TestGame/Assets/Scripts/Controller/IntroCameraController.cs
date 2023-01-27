using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{

    private float INTRO_ORBIT_DURATION = 30f;
    private float TRANSITION_ORBIT_DURATION = 3f;

    private float orbit_angle = 40f;
    private float orbit_radius = 30f;
    private float orbit_altitude = 10f;

    private Coroutine orbit_coroutine;

    private void Start() {
        HandleIntro();
    }

    public void HandleInputStartGame() => HandleTransition();

    private void HandleIntro() {
        this.EnsureCoroutineStopped(ref orbit_coroutine);
        orbit_coroutine = this.CreateAnimationRoutine(
            INTRO_ORBIT_DURATION,
            delegate (float t) {
                orbit_angle = Mathf.Lerp(0, 2 * Mathf.PI, t);
                Vector3 new_pos = orbit_radius * new Vector3(Mathf.Cos(orbit_angle), 0, Mathf.Sin(orbit_angle));
                new_pos.y = orbit_altitude;

                transform.position = new_pos;
                transform.LookAt(Vector3.zero);
            },
            delegate () {
                HandleIntro();
            }
        );
    }

    private void HandleTransition() {
        float start_angle = orbit_angle;
        float end_angle = orbit_angle + 2 * Mathf.PI;

        float start_radius = orbit_radius;
        float end_radius = 0;

        float start_altitude = orbit_altitude;
        float end_altitude = 1;

        this.EnsureCoroutineStopped(ref orbit_coroutine);
        orbit_coroutine = this.CreateAnimationRoutine(
            TRANSITION_ORBIT_DURATION,
            delegate (float t) {
                orbit_angle = Mathf.Lerp(start_angle, end_angle, t);
                orbit_radius = Mathf.Lerp(start_radius, end_radius, t);
                orbit_altitude = Mathf.Lerp(start_altitude, end_altitude, t);

                Vector3 new_pos = orbit_radius * new Vector3(Mathf.Cos(orbit_angle), 0, Mathf.Sin(orbit_angle));
                new_pos.y = orbit_altitude;

                transform.position = new_pos;
                transform.LookAt(Vector3.zero);
            },
            delegate () {
                HandleStartGame();
            }
        );
    }

    private void HandleStartGame() {
        BattleSceneManager.Instance.StartGame();
    }

}
