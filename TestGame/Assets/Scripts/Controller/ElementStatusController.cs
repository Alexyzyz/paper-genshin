using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStatusController : MonoBehaviour
{

    [SerializeField]
    private ElementStatusIconController element_status_icon_prefab;

    private ElementStatusIconController applied_element;

    public void HandleUpdateElement(GameData.Element new_element) {
        if (new_element == GameData.Element.NONE) {
            Destroy(applied_element.gameObject);
            return;
        }

        ElementStatusIconController element_status_icon_controller = Instantiate(element_status_icon_prefab, transform);
        element_status_icon_controller.Initialize(new_element);
        applied_element = element_status_icon_controller;
    }

    public void AnimateTriggeredReaction(GameData.Reaction reaction) {
        Destroy(applied_element.gameObject);
    }
    
}
