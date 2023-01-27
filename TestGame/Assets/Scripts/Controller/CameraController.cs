using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private CameraModel Model = new();

    private const float ZOOM_TRANSITION_DURATION = 0.1f;

    private Camera my_camera;
    private Transform aim_marker;

    private float y_angle = 0;
    private float x_angle = 0;
    private float sensitivity;

    private Coroutine zoom_coroutine;

    private void Awake() {
        my_camera = GetComponent<Camera>();
        aim_marker = transform.Find("AimMarker");

        sensitivity = Model.SENSITIVITY;
    }

    private void Update()
    {
        HandleMovement();
    }

    public float GetYAngle() => transform.eulerAngles.y;

    public void SetAiming(bool aiming_state) {
        SetFieldOfView(aiming_state ? Model.AIMING_FOV : Model.FOV);
        sensitivity = (aiming_state ? Model.AIM_SENSITIVITY_RATIO : 1) * Model.SENSITIVITY;
    }

    private void SetFieldOfView(float fov) {
        float start_fov = my_camera.fieldOfView;
        float end_fov = fov;
        this.EnsureCoroutineStopped(ref zoom_coroutine);
        zoom_coroutine = this.CreateAnimationRoutine(
            ZOOM_TRANSITION_DURATION,
            delegate (float t) {
                my_camera.fieldOfView = Mathf.Lerp(start_fov, end_fov, t);
            }
        );
    }

    private void HandleMovement() {
        y_angle += Time.deltaTime * sensitivity * Input.GetAxis("Mouse X");
        x_angle += Time.deltaTime * sensitivity * Input.GetAxis("Mouse Y");

        x_angle = Mathf.Clamp(x_angle, Model.MIN_X_ANGLE, Model.MAX_X_ANGLE);

        transform.eulerAngles = new Vector3(-x_angle, y_angle, 0);
    }
    
}
