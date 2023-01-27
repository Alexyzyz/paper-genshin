using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{

    public float damage;
    public GameData.Element element;

    public Attack(float damage, Element element) {
        this.damage = damage;
        this.element = element;
    }

}
