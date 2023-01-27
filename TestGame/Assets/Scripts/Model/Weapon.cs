using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon")]
public class Weapon : ScriptableObject
{

    public string title;
    [TextArea]
    public string description;
    
    public Sprite sprite;
    public float damage;
    public float mana_cost;
    public GameData.Element element;
    
}
