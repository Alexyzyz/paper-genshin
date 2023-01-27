using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel
{

    public float SENSITIVITY { get; } = 100f;
    public float AIM_SENSITIVITY_RATIO { get; } = 0.5f;
    public float FOV { get; } = 60;
    public float AIMING_FOV { get; } = 55;

    public float MIN_X_ANGLE { get; } = -90f;
    public float MAX_X_ANGLE { get; } = 75f;

}
