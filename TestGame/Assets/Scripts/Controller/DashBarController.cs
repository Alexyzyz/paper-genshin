using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBarController : MonoBehaviour
{

    private PlayerController player_controller;

    private RectTransform rect_transform;

    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
    }

    private void Start() {
        player_controller = BattleSceneManager.Instance.player_controller;
    }

    private void Update() {
        UpdateBar();
    }

    public void UpdateBar() {
        float dash_timer = player_controller.Model.dash_timer;
        float dash_cooldown = player_controller.Model.DASH_COOLDOWN;

        Vector3 new_scale = rect_transform.localScale;
        new_scale.x = dash_timer / dash_cooldown;
        rect_transform.localScale = new_scale;
    }

}
