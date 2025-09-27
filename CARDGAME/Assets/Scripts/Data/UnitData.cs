using UnityEngine;

[CreateAssetMenu(menuName = "BattleGame/Unit Data", fileName = "NewUnitData")]
public class UnitData : ScriptableObject {

    // ! Name/Reference stats....
    [Header("Name Info")]
    public string unitName;

    [Header("Image (maybe Unit Image..?)")]
    public Sprite icon;

    [Header("Prefab Reference")]
    public GameObject prefab;

    // * Basic stats each will probably have...
    [Header("Stats")]
    public int cost;
    public float cooldown;
    public float health;
    public float attack;
    public float attackRate;
    public float speed;

}
