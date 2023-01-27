using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect
{

    public float DASH_EFFECT_DURATION { get; private set; } = 0.25f;
    public int DASH_PARTICLE_COUNT_PER_FRAME { get; private set; } = 1;
    public float DASH_PARTICLE_MIN_SPAWN_DISTANCE { get; private set; } = 800f;
    public float DASH_PARTICLE_MAX_SPAWN_DISTANCE { get; private set; } = 900f;
    public float DASH_PARTICLE_MOVE_BACK_DISTANCE { get; private set; } = 1000;
    public float DASH_PARTICLE_ANIMATION_DURATION { get; private set; } = 0.33f;

}
