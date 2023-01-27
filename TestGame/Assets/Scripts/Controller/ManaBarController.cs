using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{


    private RectTransform rect_transform;
    private Text quantity_text_component;

    private void Awake()
    {
        rect_transform = transform.Find("Bar").GetComponent<RectTransform>();
        quantity_text_component = transform.Find("Quantity").GetComponent<Text>();
    }

    private void Start() {
        BattleSceneManager.Instance.player_controller.Model.OnManaChange += HandleManaBar;
    }

    public void HandleManaBar(float mana, float max_mana) {
        quantity_text_component.text = "" + (int)mana;

        Vector3 new_scale = rect_transform.localScale;
        new_scale.x = mana / max_mana;
        rect_transform.localScale = new_scale;
    }
    
}
