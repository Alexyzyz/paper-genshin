using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStatusIconController : MonoBehaviour
{

    private SpriteRenderer sprite_renderer;

    private void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        
    }

    public void Initialize(GameData.Element element) {
        sprite_renderer.sprite = ElementUtility.Instance.GetElementSprite(element);
    }

    public void HandleReaction() {

    }

    private void HandleAuraTimer() {

    }
    
}
