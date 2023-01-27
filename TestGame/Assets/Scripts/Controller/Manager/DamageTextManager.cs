using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{

    public static DamageTextManager Instance;

    [SerializeField]
    private GameObject damage_text_prefab;

    private RectTransform rect_transform;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        rect_transform = GetComponent<RectTransform>();
    }

    public void DrawDamageText(string text, Color color, int font_size, Vector3 world_space_position) {
        DamageTextController damage_text_controller = Instantiate(damage_text_prefab, rect_transform).GetComponent<DamageTextController>();
        damage_text_controller.Initialize(text, color, font_size, world_space_position);
    }

}
